using Abp.Application.Services.Dto;
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
        private readonly IRepository<Tiku, int> _tikuRepository;

        public YljztAppService(
            Abp.Domain.Repositories.IRepository<Tiku, int> repository,
            IRepository<Xuanxiang, int> xuanxiangRepository,
            IRepository<Daan, int> daanRepository,
            IRepository<Renyua1, int> renyuanRepository,
            IRepository<Tiku, int> tikuRepository
            ) : base(repository)
        {
            _xuanxiangRepository = xuanxiangRepository;
            _daanRepository = daanRepository;
            _renyuanRepository = renyuanRepository;
            _tikuRepository = tikuRepository;
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
            var maxResultCount = input.MaxResultCount;
            input.MaxResultCount = int.MaxValue;
           var result = await base.GetAll(input);

            var timuIds = result.Items.Select(i => i.Id).ToList();

            for (var i = 0; i < timuIds.Count; i++)
            {
                var index = new Random().Next(timuIds.Count);
                var temp = timuIds[i];
                timuIds[i] = timuIds[index];
                timuIds[index] = temp;
            }
            timuIds = timuIds.OrderByDescending(i=>Guid.NewGuid()).Take(maxResultCount).ToList();

            var xuanxiangs = _xuanxiangRepository.GetAll()
                .Where(r => timuIds.Contains(r.TimuId))
                .ToList();

            xuanxiangs.Sort((l, r) => l.Name.CompareTo(r.Name));
            var tihao = 1;
            result.Items = result.Items.Where(i => timuIds.Contains(i.Id)).ToList();
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

        public List<TimuRenshu> GetTimuRenshu()
        {
            var result = new List<TimuRenshu>();
            var query =
                from d in _daanRepository.GetAll()
                join x in _xuanxiangRepository.GetAll()
                    on d.XuanxiangId equals x.Id
                join t in _tikuRepository.GetAll()
                    on d.TimuId equals t.Id
                select new { Tihao =t.TiHao, Timu=t.TiMu, IsRight = x.IsRight };

            var list = query.ToList();

            Dictionary<int, List<int>> tongji = new Dictionary<int, List<int>>();

            foreach (var item in list)
            {
                if (!tongji.ContainsKey(item.Tihao))
                {
                    tongji.Add(item.Tihao, new List<int> { 0, 0 });
                }

                if (item.IsRight)
                {
                    tongji[item.Tihao][0]++;
                }
                tongji[item.Tihao][1]++;
            }

            foreach (var item in tongji)
            {
                var timu = list.FirstOrDefault(l => l.Tihao == item.Key);
                result.Add(new TimuRenshu
                {
                    Tihao = item.Key,
                    Timu = timu.Timu,
                    Renshu = tongji[item.Key][0] 
                });
            }
            return result.OrderByDescending(r => r.Renshu).Take(10).ToList();
        }

        public List<XueyuanZhengquelv> GetXueyuanZhengquelv()
        {
            var result = new List<XueyuanZhengquelv>();

            var query =
                 from d in _daanRepository.GetAll()
                 join x in _xuanxiangRepository.GetAll()
                     on d.XuanxiangId equals x.Id
                join r in _renyuanRepository.GetAll()
                    on d.RenyuanId equals r.Id
                 select new { Xueyuan=r.Xueyua, IsRight = x.IsRight };
            var list = query.ToList();

            Dictionary<string, List<int>> tongji = new Dictionary<string, List<int>>();

            foreach (var item in list)
            {
                if (!tongji.ContainsKey(item.Xueyuan))
                {
                    tongji.Add(item.Xueyuan, new List<int> { 0 ,0});
                }

                if (item.IsRight)
                {
                    tongji[item.Xueyuan][0]++;
                }
                tongji[item.Xueyuan][1]++;
            }

            foreach(var item in tongji)
            {
                result.Add(new XueyuanZhengquelv
                {
                    Xuyuan = item.Key,
                    Zhengquelv = 100.0 * tongji[item.Key][0] / tongji[item.Key][1] 
                }) ;
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

        public List<XueyuanCanyu> GetXueyuanCanyu()
        {
            var query =
                 from d in _daanRepository.GetAll()
                 join x in _xuanxiangRepository.GetAll()
                     on d.XuanxiangId equals x.Id
                 join r in _renyuanRepository.GetAll()
                     on d.RenyuanId equals r.Id
                 select new { Xueyuan = r.Xueyua };
            var list = from q in query
                       group q by q.Xueyuan
                        into g
                       select new XueyuanCanyu { Xueyuan=g.Key, Renshu = g.Count()};

            return list.ToList();
        }
    }
}
