using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppEditions.Dto
{
    public class CreateAppEditionInput : ICustomValidate
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [StringLength(10)]
        public string Version { get; set; }
        /// <summary>
        /// 关于
        /// </summary>
        [StringLength(500)]
        [Url]
        public string AboutUrl { get; set; }
        /// <summary>
        /// 安装包
        /// </summary>
        public Guid? InstallationPackage { get; set; }
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool IsMandatoryUpdate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500)]
        public string Describe { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            try
            {
                System.Version.Parse(Version);
            }
            catch (Exception e)
            {
                context.Results.Add(new ValidationResult(e.Message));
            }
        }
    }
}
