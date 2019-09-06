using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.OrganizationUnits.Dto;
using QC.MF.Roles.Dto;
using System.Web.Mvc;

namespace QC.MF.OrganizationUnits
{
    /// <summary>
    /// 组织单元管理
    /// </summary>
    public interface IOrganizationUnitAppService : IApplicationService
    {
        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits();

        /// <summary>
        /// 创建组织机构
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input);

        /// <summary>
        /// 修改组织机构信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input);

        /// <summary>
        /// 移动组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input);

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task DeleteOrganizationUnit(EntityDto<long> input);

        /// <summary>
        /// 获取组织机构下的所有人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input);

        /// <summary>
        /// 获取可加入某组织单元的所有人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        PagedResultDto<OrganizationUnitUserDto> GetOrganizationUnitJoinableUserList(
            GetOrganizationUnitJoinableUserListInput input);

        /// <summary>
        /// 将用户添加到组织机构中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task AddUserToOrganizationUnit(UsersToOrganizationUnitInput input);

        /// <summary>
        /// 从组织机构中移除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task RemoveUserFromOrganizationUnit(UsersToOrganizationUnitInput input);

        /// <summary>
        /// 用户是否属于组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input);

        /// <summary>
        /// 获取用户所在组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<long>> GetUserOrganizationUnits(UserIdInput input);

        /// <summary>
        /// 移除用户的全部组织机构
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveAllOrganizationUnit(long userId);
    }
}
