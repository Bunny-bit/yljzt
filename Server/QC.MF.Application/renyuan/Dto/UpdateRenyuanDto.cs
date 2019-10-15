using Abp.Application.Services.Dto;
using QC.MF.Demos.Dto;
using QC.MF.Renyuan.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.renyuan.Dto
{
    public class UpdateRenyuanDto : CreateRenyuanDto, IEntityDto<int>
    {
        public int Id { get; set; }
    }
}
