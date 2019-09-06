using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using QC.MF.Authorization;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;
using QC.MF.Roles.Dto;
using QC.MF.Users.Dto;
using Microsoft.AspNet.Identity;
using Abp.Authorization.Roles;
using Abp.Extensions;
using Abp.Configuration;
using Abp.AutoMapper;
using QC.MF.Authorization.Permissions.Dto;
using QC.MF.Authorization.Permissions;
using Abp.Runtime.Session;
using Abp.UI;
using System.Diagnostics;
using Abp.Zero.Configuration;
using System.Data.Entity;
using Abp.Linq.Extensions;
using QC.MF.OrganizationUnits;
using System;
using System.Web.WebPages;
using Abp.Net.Mail.Smtp;
using QC.MF.Notifications;
using Abp.Notifications;
using QC.MF.Configuration;
using QC.MF.OrganizationUnits.Dto;
using QC.MF.SMSs;
using DataExporting;
using DataExporting.Dto;

namespace QC.MF.Users
{
    public class UserAppService : MFAppServiceBase, IUserAppService
    {
        private readonly RoleManager _roleManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;

        private readonly IOrganizationUnitAppService _organizationUnitApp;
        private readonly IPermissionAppService _permissionAppService;

        private readonly ISmtpEmailSender _emailSender;
        private readonly ISMSSenderManager _smsSender;

        private readonly IAppFolders _appFolders;

        public UserAppService(
            RoleManager roleManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IOrganizationUnitAppService organizationUnitApp, 
            IPermissionAppService permissionAppService, 
            ISmtpEmailSender emailSender,
            IAppFolders appFolders, ISMSSenderManager smsSender)
        {
            _roleManager = roleManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _organizationUnitApp = organizationUnitApp;
            _permissionAppService = permissionAppService;
            _emailSender = emailSender;
            _appFolders = appFolders;
            _smsSender = smsSender;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Lookup, PermissionNames.Pages_Administration_Users)]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Lookup, PermissionNames.Pages_Administration_Users)]
        public async Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input)
        {
            IQueryable<User> query = QueryUser(input);

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = users.MapTo<List<UserListDto>>();
            await FillRoleNames(userListDtos);

            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
                );
        }

        private IQueryable<User> QueryUser(GetUsersInput input)
        {
            var query = UserManager.Users
                .Include(u => u.Roles)
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        //u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter) ||
                        u.PhoneNumber.Contains(input.Filter)
                )
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), u => u.Name.Contains(input.Name))
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), u => u.UserName.Contains(input.UserName))
                .WhereIf(!input.PhoneNumber.IsNullOrWhiteSpace(), u => u.PhoneNumber.Contains(input.PhoneNumber));

            if (!input.Permission.IsNullOrWhiteSpace())
            {
                query = (from user in query
                         join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                         from ur in urJoined.DefaultIfEmpty()
                         join up in _userPermissionRepository.GetAll() on new { UserId = user.Id, Name = input.Permission } equals new { up.UserId, up.Name } into upJoined
                         from up in upJoined.DefaultIfEmpty()
                         join rp in _rolePermissionRepository.GetAll() on new { RoleId = ur.RoleId, Name = input.Permission } equals new { rp.RoleId, rp.Name } into rpJoined
                         from rp in rpJoined.DefaultIfEmpty()
                         where (up != null && up.IsGranted) || (up == null && rp != null)
                         group user by user into userGrouped
                         select userGrouped.Key);
            }

            return query;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users)]
        public async Task<FileDto> GetUsersToExcel(GetUsersInput input)
        {
            IQueryable<User> query = QueryUser(input);
            var users = await query.ToListAsync();
            var userListDtos = users.MapTo<List<UserListDto>>();

            await FillRoleNames(userListDtos);
            return new ExcelExporter().ExportToFile(userListDtos,_appFolders.TempFileDownloadFolder);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            //Getting all available roles
            var userRoleDtos = (await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync());

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos
            };

            if (!input.Id.HasValue)
            {
                //Creating a new user
                output.User = new UserEditDto
                {
                    IsActive = true,
                    ShouldChangePasswordOnNextLogin = true,
                    //IsTwoFactorEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                    IsLockoutEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled)
                };

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                //Editing an existing user
                var user = await UserManager.GetUserByIdAsync(input.Id.Value);

                output.User = user.MapTo<UserEditDto>();
                output.ProfilePictureId = user.ProfilePictureId;
                output.OrganizationIds = await _organizationUnitApp.GetUserOrganizationUnits(new UserIdInput{UserId = input.Id ?? 0 });
                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(input.Id.Value, userRoleDto.RoleName);
                }
            }
            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var permissions = _permissionAppService.GetAllPermissionTree();
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                Permissions = permissions,
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task ResetUserSpecificPermissions(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            await UserManager.ResetAllPermissionsAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Create, PermissionNames.Pages_Administration_Users_Edit)]
        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);
            }
            else
            {
                await CreateUserAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task DeleteUser(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task BatchDeleteUser(CommonDto.ArrayDto<long> input)
        {
            if (input.Value == null) { return; }
            foreach (var id in input.Value)
            {
                await DeleteUser(new EntityDto<long>(id));
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Active)]
        public async Task ToggleActiveStatus(EntityDto<long> input)
        {
            var user = await UserManager.FindByIdAsync(input.Id);
            user.IsActive = !user.IsActive;
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Unlock)]
        public async Task UnlockUser(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            user.Unlock();
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Unlock)]
        public async Task BatchUnlockUser(CommonDto.ArrayDto<long> input)
        {
            foreach (var id in input.Value)
            {
                await UnlockUser(new EntityDto<long> { Id=id});
            }
        }
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Active)]
        public async Task BatchActiveUser(BatchActiveUserInput input)
        {
            var users = await UserManager.Users.Where(x => input.Ids.Contains(x.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsActive=input.IsActive;
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await UserManager.FindByIdAsync(input.User.Id.Value);

            //Update user properties
            input.User.MapTo(user); //Passwords is not mapped (see mapping configuration)

            if (input.SetRandomPassword)
            {
                input.User.Password = User.CreateRandomPassword();
            }

            if (!input.User.Password.IsNullOrEmpty())
            {
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
            }

            CheckErrors(await UserManager.UpdateAsync(user));

            //Update roles
            CheckErrors(await UserManager.SetRoles(user, input.AssignedRoleNames));

            await SetOrganization(user,input.Organizations);

            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                //await _userEmailer.SendEmailActivationLinkAsync(user, input.User.Password);
            }
        }
        private async Task  SetOrganization(User user,int[] organizations)
        {
            if (organizations == null)
            {
                return;
            }
            await _organizationUnitApp.RemoveAllOrganizationUnit(user.Id);
            UnitOfWorkManager.Current.SaveChanges();
            foreach (var org in organizations)
            {
                await _organizationUnitApp.AddUserToOrganizationUnit(
                    new OrganizationUnits.Dto.UsersToOrganizationUnitInput() { OrganizationUnitId = org, UserIdListStr = user.Id.ToString() });
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Create)]
        protected virtual async Task CreateUserAsync(CreateOrUpdateUserInput input)
        {
            //if (AbpSession.TenantId.HasValue)
            //{
            //    await _userPolicy.CheckMaxUserCountAsync(AbpSession.GetTenantId());
            //}

            var user = input.User.MapTo<User>(); //Passwords is not mapped (see mapping configuration)
            user.TenantId = AbpSession.TenantId;
            
            //if (!input.User.Password.IsNullOrEmpty())
            {
                CheckErrors(await UserManager.PasswordValidator.ValidateAsync(input.User.Password));
            }
            //else
            //{
            //    input.User.Password = User.CreateRandomPassword();
            //}

            user.Password = new PasswordHasher().HashPassword(input.User.Password);
            user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

            //Assign roles
            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }
            if (!string.IsNullOrWhiteSpace(user.PhoneNumber) && (await UserManager.FindUserByPhoneNumberAsync(user.PhoneNumber)) != null)
            {
                throw new UserFriendlyException("当前手机号已被注册，创建用户失败");
            }
            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            await SetOrganization(user, input.Organizations);

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
            try
            {
                if (input.SendActivationEmail && !string.IsNullOrWhiteSpace(input.User.EmailAddress))
                {
                    var body = $"您好，系统已为您创建账号，用户名:{input.User.UserName}, 验证码:{input.User.Password}，感谢您使用本系统";
                    if (!input.User.IsActive)
                    {
                        body = $"您好，系统已为您创建账号，用户名:{input.User.UserName}, 验证码:{input.User.Password}，" +
                               "首次登陆需要激活帐号，感谢您使用本系统。";
                    }
                    var subject = "账号创建通知";
                    await _emailSender.SendAsync(input.User.EmailAddress, subject, body);
                }
                if (input.SendActivationMessage && !string.IsNullOrWhiteSpace(input.User.PhoneNumber))
                {
                    var body = $"您好，系统已为您创建账号，用户名:{input.User.UserName}, 密码:{input.User.Password}，感谢您使用本系统";
                    if (!input.User.IsActive)
                    {
                        body = $"您好，系统已为您创建账号，用户名:{input.User.UserName}, 密码:{input.User.Password}，首次登陆需要激活帐号，感谢您使用本系统。";
                    }
                    await _smsSender.Sender(input.User.PhoneNumber, body);
                }
            }
            catch(Exception ex)
            {
                //ignore
            }
        }

        private async Task FillRoleNames<T>(List<T> userListDtos) where T : UserListDto
        {
            /* This method is optimized to fill role names to given list. */

            var distinctRoleIds = (
                from userListDto in userListDtos
                from userListRoleDto in userListDto.Roles
                select userListRoleDto.RoleId
                ).Distinct();

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
            {
                roleNames[roleId] = (await _roleManager.GetRoleByIdAsync(roleId)).DisplayName;
            }

            foreach (var userListDto in userListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                {
                    userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];
                }

                userListDto.Roles = userListDto.Roles.OrderBy(r => r.RoleName).ToList();
            }
        }
        [AbpAuthorize]
        public async Task UpdateCurrentUser(UpdateCurrentUserInput input)
        {
            var id = AbpSession.GetUserId();
            var user = await UserManager.FindByIdAsync(id);

            //Update user properties
            input.MapTo(user); //Passwords is not mapped (see mapping configuration)

            CheckErrors(await UserManager.UpdateAsync(user));
        }
    }
} 
