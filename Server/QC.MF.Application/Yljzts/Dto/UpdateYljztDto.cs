using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Yljzts.Dto
{
    [AutoMap(typeof(Tiku))]
   public class UpdateYljztDto : CreateYljztDto, IEntityDto<int>
    {
        public int Id { get ; set; }
    }
}
