using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppEditions.Dto
{
    [AutoMap(typeof(AndroidAppEdition))]
    public class UpdateAndroidAppEditionInput : CreateAndroidAppEditionInput, IEntityDto
    {
        public int Id { get; set; }
    }
}
