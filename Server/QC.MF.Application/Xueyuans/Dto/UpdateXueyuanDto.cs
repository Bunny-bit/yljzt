using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xueyuans.Dto
{
    [AutoMap(typeof(Xueyuan.Xueyuan))]
    public class UpdatexueyuanDto : CreatexueyuanDto, IEntityDto<int>
    {
        public int Id { get; set; }
    }
}
