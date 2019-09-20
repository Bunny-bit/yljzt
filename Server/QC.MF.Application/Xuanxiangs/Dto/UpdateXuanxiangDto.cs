using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xuanxiangs.Dto
{
    [AutoMap(typeof(Xuanxiang))]
    public class UpdateXuanxiangDto : CreateXuanxiangDto, IEntityDto<int>
    {
        public int Id { get ; set; }
    }
}
