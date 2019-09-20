using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using QC.MF.CommonDto;
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

        public YljztAppService(
            Abp.Domain.Repositories.IRepository<Tiku, int> repository,
            IRepository<Xuanxiang, int> xuanxiangRepository
            ) : base(repository)
        {
            _xuanxiangRepository = xuanxiangRepository;
        }

        public async override Task<PagedResultDto<GetListYljztDto>> GetAll(PagedSortedAndFilteredInputDto input)
        {
            var result = await base.GetAll(input);

            var timuIds = result.Items.Select(r => r.Id).ToList();

            var xuanxiangs = _xuanxiangRepository.GetAll()
                .Where(r => timuIds.Contains(r.TimuId))
                .ToList();

            foreach (var xuanxiang in xuanxiangs)
            {
                var timu = result.Items.First(e => e.Id == xuanxiang.TimuId);
                timu.Xuanxiangs.Add(new Xuanxiangs.Dto.GetListXuanxiangDto {
                    Id = xuanxiang.Id,
                     TimuId = xuanxiang.TimuId,
                    Name = xuanxiang.Name,
                    Neirong = xuanxiang.Neirong
                });
            }
            return result;
        }
    }
}
