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
    public class GetListDemoDto : CreateDemoDto, IEntityDto<int>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string _CreationTime => CreationTime?.ToString("yyyy年MM月dd日");

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public string _LastModificationTime => LastModificationTime?.ToString("yyyy年MM月dd日");

        public int Id { get; set; }
    }
}
