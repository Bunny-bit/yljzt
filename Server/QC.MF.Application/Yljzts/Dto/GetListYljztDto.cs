using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Xuanxiangs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Yljzts.Dto
{
    [AutoMap(typeof(Tiku))]
    public class GetListYljztDto : CreateYljztDto, IEntityDto<int>
    {
        public int Id { get ; set; }

        public List<GetListXuanxiangDto> Xuanxiangs { get; set; }

        public GetListYljztDto()
        {
            Xuanxiangs = new List<GetListXuanxiangDto>();
        }
    }
}
