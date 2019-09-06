using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.Roles.Dto;
using QC.MF.Users.Dto;
using DataExporting.Dto;
using QC.MF.CommonDto;

namespace QC.MF.Users
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取全部的角色
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<RoleDto>> GetRoles();

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);

        /// <summary>
        /// 导出用户信息到Excel文件
        /// </summary>
        /// <returns></returns>
        Task<DataExporting.Dto.FileDto> GetUsersToExcel(GetUsersInput input);

        /// <summary>
        /// 获取编辑时需要的单个用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);

        /// <summary>
        /// 获取编辑时需要的用户权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);

        /// <summary>
        /// 重置用户权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ResetUserSpecificPermissions(EntityDto<long> input);

        /// <summary>
        /// 修改用户权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        /// <summary>
        /// 创建或编辑用户，Id为空时创建，否则编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteUser(EntityDto<long> input);

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchDeleteUser(ArrayDto<long> input);

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UnlockUser(EntityDto<long> input);

        /// <summary>
        /// 批量解锁用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchUnlockUser(ArrayDto<long> input);

        /// <summary>
        /// 批量激活用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchActiveUser(BatchActiveUserInput input);


        /// <summary>
        /// 用户修改自己的信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateCurrentUser(UpdateCurrentUserInput input);

        /// <summary>
        /// 切换用户启用禁用状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ToggleActiveStatus(EntityDto<long> input);
    }
}
