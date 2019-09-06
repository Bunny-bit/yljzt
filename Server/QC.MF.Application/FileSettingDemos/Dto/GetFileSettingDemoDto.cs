using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Demos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Demos.Dto
{
    [AutoMap(typeof(FileSettingDemo))]
    public class GetFileSettingDemoDto : SetFileSettingDemoDto, IEntityDto<int>
    {

        public virtual DateTime? LastModificationTime { get; set; }
        public int Id { get; set; }
    }
}
