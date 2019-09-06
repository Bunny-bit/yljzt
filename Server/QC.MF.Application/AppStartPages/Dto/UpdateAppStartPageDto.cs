using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppStartPages.Dto
{
    [AutoMap(typeof(AppStartPage))]
    public class UpdateAppStartPageDto:CreateAppStartPageDto, IEntityDto<int>
    {
        public int Id { get; set; }
    }
}
