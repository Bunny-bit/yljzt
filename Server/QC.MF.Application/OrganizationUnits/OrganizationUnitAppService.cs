using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Organizations;
using QC.MF.Authorization.Users;
using QC.MF.OrganizationUnits.Dto;
using Abp.Linq.Extensions;
using QC.MF.Authorization;
using System.Data.Entity;
using Abp.UI;

namespace QC.MF.OrganizationUnits
{
    /// <summary>
    /// 组织机构管理
    /// </summary>
    public class OrganizationUnitAppService : MFAppServiceBase, IOrganizationUnitAppService
    {

        private readonly OrganizationUnitManager _organizationUnitManager;

        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        private readonly IRepository<User, long> _userRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="organizationUnitManager"></param>
        /// <param name="organizationUnitRepository"></param>
        /// <param name="userOrganizationUnitRepository"></param>
        /// <param name="userRepository"></param>
        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<User, long> userRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
        }

        public async Task RemoveAllOrganizationUnit(long userId)
        {
            await _userOrganizationUnitRepository.DeleteAsync(x => x.UserId == userId);
        }

        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits()
        {
            var query =
                from ou in _organizationUnitRepository.GetAll()
                join uou in _userOrganizationUnitRepository.GetAll() on ou.Id equals uou.OrganizationUnitId into g
                select new { ou, memberCount = g.Count() };

            var items = await query.ToListAsync();
            var ous = items.Select(item =>
                {
                    var dto = item.ou.MapTo<OrganizationUnitDto>();
                    dto.MemberCount = item.memberCount;
                    return dto;
                }).OrderBy(i => i.Id).ToList();
            var result = new List<OrganizationUnitDto>(ous.Where(i => !i.ParentId.HasValue));
            BuildOrganizationUnitTree(ref ous, result);
            return new ListResultDto<OrganizationUnitDto>(result);
        }

        private void BuildOrganizationUnitTree(ref List<OrganizationUnitDto> source, List<OrganizationUnitDto> parents)
        {
            source = source.Except(parents).ToList();
            foreach (var parent in parents)
            {
                var children = source.Where(s => s.ParentId == parent.Id).ToList();
                parent.Children = children;
                if (children.Count > 0)
                {
                    BuildOrganizationUnitTree(ref source, children);
                }
            }
        }
        /// <summary>
        /// 创建组织机构
        /// </summary>
        /// <param name="input"></param>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId == 0 ? null : input.ParentId);

            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return organizationUnit.MapTo<OrganizationUnitDto>();
        }

        private async Task<OrganizationUnitDto> CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = organizationUnit.MapTo<OrganizationUnitDto>();
            dto.MemberCount = await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }

        /// <summary>
        /// 修改组织机构信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            organizationUnit.DisplayName = input.DisplayName;

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnitDto(organizationUnit);
        }

        /// <summary>
        /// 移动组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            var children = await GetAllChildren(input.Id);
            if (children.Contains(input.NewParentId ?? 0))
            {
                throw new UserFriendlyException("检查到嵌套层级结构，保存失败");
            }

            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnitDto(
                await _organizationUnitRepository.GetAsync(input.Id)
            );
        }
        private async Task<List<long>> GetAllChildren(long organizationId)
        {
            var menuList = (await _organizationUnitRepository.GetAllListAsync()).MapTo<IList<OrganizationUnitDto>>();
            var result = new List<long>();
            var parentIds = new List<long> { organizationId };
            List<long> children;
            do
            {
                children = menuList.Where(m => parentIds.Contains(m.ParentId ?? 0)).Select(m => m.Id).ToList();
                parentIds = children;
                result.AddRange(children);
            } while (children.Count > 0);
            return result;
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            await _organizationUnitManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 获取组织机构下的所有人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits)]
        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            var ouIds = new List<long>{input.Id};
            if (input.IsRecursiveSearch)
            {
                var ou = await _organizationUnitRepository.GetAsync(input.Id);
                if(ou != null && !string.IsNullOrEmpty(ou.Code))
                {
                    ouIds.AddRange(_organizationUnitRepository.GetAll()
                        .Where(o=>o.Code.StartsWith(ou.Code))
                        .Select(o=>o.Id));
                }
            }

            var query = from uou in _userOrganizationUnitRepository.GetAll()
                join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                join user in UserManager.Users on uou.UserId equals user.Id
                where ouIds.Contains(uou.OrganizationUnitId)
                select new { uou, user };
            if (!string.IsNullOrEmpty(input.NameFilter))
            {
                query = query.Where(q => q.user.UserName.Contains(input.NameFilter));
            }
            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitUserListDto>(
                totalCount,
                items.Select(item =>
                {
                    var dto = item.user.MapTo<OrganizationUnitUserListDto>();
                    dto.AddedTime = item.uou.CreationTime.ToString("yyyy-MM-dd HH:dd:ss");
                    return dto;
                }).ToList());
        }

        /// <summary>
        /// 获取可加入某组织单元的所有人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits)]
        public PagedResultDto<OrganizationUnitUserDto> GetOrganizationUnitJoinableUserList(GetOrganizationUnitJoinableUserListInput input)
        {
            var userQuery =
                from u in _userRepository.GetAll()
                join ouu1 in _userOrganizationUnitRepository.GetAll().Where(q => q.OrganizationUnitId == input.Id)
                on u.Id equals ouu1.UserId
                into ouu2
                from ouu in ouu2.DefaultIfEmpty()
                where ouu == null
                select u;
            if (!string.IsNullOrEmpty(input.Filter))
            {
                userQuery = userQuery.Where(u =>
                    u.UserName.Contains(input.Filter)
                    || u.Name.Contains(input.Filter)
                    || u.PhoneNumber.Contains(input.Filter)
                    || u.EmailAddress.Contains(input.Filter));
            }
            var finalQuery = userQuery.OrderBy(u => u.Id).Skip(input.SkipCount).Take(input.MaxResultCount);
            var result = new PagedResultDto<OrganizationUnitUserDto>
            {
                TotalCount = userQuery.Count(),
                Items = finalQuery.ToList().MapTo<List<OrganizationUnitUserDto>>(),
            };
            return result;
        }

        /// <summary>
        /// 将用户添加到组织机构中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task AddUserToOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            var userIdList = input.UserIdListStr.ToIntList();
            foreach (var userId in userIdList)
            {
                await UserManager.AddToOrganizationUnitAsync(userId, input.OrganizationUnitId);
            }
            
        }

        /// <summary>
        /// 从组织机构中移除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task RemoveUserFromOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            var userIdList = input.UserIdListStr.ToIntList();
            foreach (var userId in userIdList)
            {
                await UserManager.RemoveFromOrganizationUnitAsync(userId, input.OrganizationUnitId);
            }
            
        }

        /// <summary>
        /// 用户是否属于组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input)
        {
            return await UserManager.IsInOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        /// <summary>
        /// 获取用户所在组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task<List<long>> GetUserOrganizationUnits(UserIdInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var organizations = await UserManager.GetOrganizationUnitsAsync(user);
            return organizations.Select(o=>o.Id).ToList();
        }
    }
}
