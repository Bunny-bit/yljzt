﻿using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using QC.MF.CommonDto;
using QC.MF.Daans;
using QC.MF.Renyua;
using QC.MF.Xuanxiangs;
using QC.MF.Yljzts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Yljzts
{
    public class YljztAppService : AsyncMFCrudAppService<Tiku, GetListYljztDto, PagedSortedAndFilteredInputDto, CreateYljztDto, UpdateYljztDto>, IYljztAppService
    {

        private readonly IRepository<Xuanxiang, int> _xuanxiangRepository;
        private readonly IRepository<Daan, int> _daanRepository;
        private readonly IRepository<Renyua1, int> _renyuanRepository;

        public YljztAppService(
            Abp.Domain.Repositories.IRepository<Tiku, int> repository,
            IRepository<Xuanxiang, int> xuanxiangRepository,
            IRepository<Daan, int> daanRepository,
            IRepository<Renyua1, int> renyuanRepository
            ) : base(repository)
        {
            _xuanxiangRepository = xuanxiangRepository;
            _daanRepository = daanRepository;
            _renyuanRepository = renyuanRepository;
        }

        public async override Task<PagedResultDto<GetListYljztDto>> GetAll(PagedSortedAndFilteredInputDto input)
        {
            var result = await base.GetAll(input);

            var timuIds = result.Items.Select(r => r.Id).ToList();

            var xuanxiangs = _xuanxiangRepository.GetAll()
                .Where(r => timuIds.Contains(r.TimuId))
                .ToList();

            xuanxiangs.Sort((l, r) => l.Name.CompareTo(r.Name));
            foreach (var xuanxiang in xuanxiangs)
            {
                var timu = result.Items.First(e => e.Id == xuanxiang.TimuId);
                timu.Xuanxiangs.Add(new Xuanxiangs.Dto.GetListXuanxiangDto {
                    Id = xuanxiang.Id,
                     TimuId = xuanxiang.TimuId,
                    Name = xuanxiang.Name,
                    Neirong = xuanxiang.Neirong,
                    IsRight=xuanxiang.IsRight
                });
            }
            return result;
        }


        public async Task<PagedResultDto<GetListYljztDto>> GetDajuan(PagedSortedAndFilteredInputDto input)
        {

            var result = await base.GetAll(input);

            var timuIds =
                (
                    from m in result.Items
                    orderby Guid.NewGuid().ToString()
                    select m.Id
                ).Take(input.MaxResultCount)
                .ToList();

            var xuanxiangs = _xuanxiangRepository.GetAll()
                .Where(r => timuIds.Contains(r.TimuId))
                .ToList();

            xuanxiangs.Sort((l, r) => l.Name.CompareTo(r.Name));
            var tihao = 1;
            foreach (var timu in result.Items)
            {
                timu.TiHao = tihao++;
            }
            foreach (var xuanxiang in xuanxiangs)
            {
                var timu = result.Items.First(e => e.Id == xuanxiang.TimuId);
                timu.Xuanxiangs.Add(new Xuanxiangs.Dto.GetListXuanxiangDto
                {
                    Id = xuanxiang.Id,
                    TimuId = xuanxiang.TimuId,
                    Name = xuanxiang.Name,
                    Neirong = xuanxiang.Neirong,
                    IsRight = xuanxiang.IsRight
                });
            }
            return result;
        }

        public List<ZhengQueShuZuiGao> GetZhengQueShuZuiGao()
        {

            var result = new List<ZhengQueShuZuiGao>();

            var query =
                 from d in _daanRepository.GetAll()
                 join x in _xuanxiangRepository.GetAll()
                     on d.XuanxiangId equals x.Id
                 where x.IsRight
                 select new {Id = d.RenyuanId};

            var groupQuery = 
                from q in query.ToList()
                group q by q.Id 
                    into g
                select new { Id = g.Key, Total = g.Count() };

            var totals = groupQuery.ToList().OrderByDescending(r => r.Total).Take(5).ToList();

            if (totals.Count == 0) {
                return result;
            }
            var renyuanIds = totals.Select(r => r.Id).ToList();
            var renyuans = _renyuanRepository.GetAll()
               .Where(ry => renyuanIds.Contains(ry.Id))
               .ToList();

            foreach (var t in totals)
            {
                result.Add(new ZhengQueShuZuiGao
                {
                    Xingming = renyuans.First(r => r.Id == t.Id).Name,
                    Timu = t.Total
                });
            }
            return result;
        }
    }
}
