using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Demos.Dto;
using QC.MF.Renyuan;
using QC.MF.Renyuan.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.renyuan.Dto
{
    [AutoMap(typeof(Renyuan.Renyuan1))]
     public class GetListRenyuanDto : CreateRenyuanDto, IEntityDto<int>
    {
        public int Id { get; set ; }
    }
}
