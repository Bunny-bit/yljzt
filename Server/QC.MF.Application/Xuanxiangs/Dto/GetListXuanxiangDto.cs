using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace QC.MF.Xuanxiangs.Dto
{
    [AutoMap(typeof(Xuanxiang))]
    public class GetListXuanxiangDto : CreateXuanxiangDto, IEntityDto<int>
    {
        public int Id { get ; set ; }
    }
}
