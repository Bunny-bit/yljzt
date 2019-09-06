using Abp.Application.Services.Dto;

namespace QC.MF.Roles.Dto
{
    public class GetRolesInput 
    {
        /// <summary>
        /// 权限过滤
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
    }
}
