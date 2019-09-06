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
    public class CreateDemoDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string LongText { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActivate { get; set; }
        

        /// <summary>
        /// 顺序
        /// </summary>
        public double Sort { get; set; }

        /// <summary>
        /// 权
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishTime { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Location Location { get; set; }
    }
}
