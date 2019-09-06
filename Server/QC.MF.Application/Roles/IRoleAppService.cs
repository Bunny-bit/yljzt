using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.Roles.Dto;

namespace QC.MF.Roles
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public interface IRoleAppService : IApplicationService
    {

        /// <summary>
        /// 获取角色列表
        /// </summary>
        Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input);

        /// <summary>
        /// 获取编辑时需要的角色信息
        /// </summary>
        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input);

        /// <summary>
        /// 创建或修改角色
        /// </summary>
        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);

        /// <summary>
        /// 删除角色
        /// </summary>
        Task DeleteRole(EntityDto input);
    }
}
