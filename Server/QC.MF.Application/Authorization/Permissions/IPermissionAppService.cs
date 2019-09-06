using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using QC.MF.Authorization.Permissions.Dto;

namespace QC.MF.Authorization.Permissions
{
    /// <summary>
    /// 权限服务端接口
    /// </summary>
    public interface IPermissionAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns></returns>
        List<PermissionDto> GetAllPermissionTree();
        /// <summary>
        /// 获取登录用户所有权值
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetUserPermissions();
    }
}
