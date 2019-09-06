using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.UI;
using QC.MF.Authorization;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;
using QC.MF.Roles.Dto;
using Microsoft.AspNet.Identity;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System.Data.Entity;
using Abp.AutoMapper;
using QC.MF.Authorization.Permissions.Dto;
using System.Diagnostics;
using QC.MF.Authorization.Permissions;

namespace QC.MF.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RoleAppService :MFAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;

        public RoleAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles, PermissionNames.Pages_Administration_Roles_Lookup)]
        public async Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), r=>r.DisplayName.Contains(input.Filter))
                .ToListAsync();

            return new ListResultDto<RoleListDto>(roles.MapTo<List<RoleListDto>>());
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto;

            if (input.Id.HasValue) //Editing existing role?
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = role.MapTo<RoleEditDto>();
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Role.Id.HasValue)
            {
                await UpdateRoleAsync(input);
            }
            else
            {
                await CreateRoleAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Delete)]
        public async Task DeleteRole(EntityDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Edit)]
        protected virtual async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            Debug.Assert(input.Role.Id != null, "input.Role.Id should be set.");

            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);
            role.DisplayName = input.Role.DisplayName;
            role.IsDefault = input.Role.IsDefault;
            CheckErrors(await _roleManager.UpdateAsync(role));
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create)]
        protected virtual async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = new Role(AbpSession.TenantId, input.Role.DisplayName) { IsDefault = input.Role.IsDefault };
            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(grantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}
