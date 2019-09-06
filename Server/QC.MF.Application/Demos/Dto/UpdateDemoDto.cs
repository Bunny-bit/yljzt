using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Demos.Dto
{
    [AutoMap(typeof(Demo))]
    public class UpdateDemoDto:CreateDemoDto, IEntityDto<int>
    {
        public int Id { get; set; }
    }
}
