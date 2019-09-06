using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Zero;
using Microsoft.AspNet.Identity;
using QC.MF.Authorization.Roles;
using System;
using Abp.Threading;
using Abp;

namespace QC.MF.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public UserManager(
            UserStore userStore,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            ISettingManager settingManager,
            IdentityEmailMessageService emailService,
            IUserTokenProviderAccessor userTokenProviderAccessor)
            : base(
                  userStore,
                  roleManager,
                  permissionManager,
                  unitOfWorkManager,
                  cacheManager,
                  organizationUnitRepository,
                  userOrganizationUnitRepository,
                  organizationUnitSettings,
                  localizationManager,
                  emailService,
                  settingManager,
                  userTokenProviderAccessor)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                return AbpIdentityResult.Failed(string.Format(L("Identity.DuplicateUserName"), userName));
            }

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return IdentityResult.Success;
            }
            user = await FindByEmailAsync(emailAddress);
            if (user != null && user.Id != expectedUserId)
            {
                return AbpIdentityResult.Failed(string.Format(L("Identity.DuplicateEmail"), emailAddress));
            }

            return IdentityResult.Success;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }

        public async Task<User> FindUserByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || !phoneNumber.IsPhoneNumber())
            {
                throw new UserFriendlyException("手机号输入有误");
            }
            var result = await Users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }


        public async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
                {
                    return await FindByIdAsync(userIdentifier.UserId);
                }
            }
        }

        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new ApplicationException("There is no user: " + userIdentifier);
            }

            return user;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        public override async Task<IdentityResult> UpdateAsync(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.PhoneNumber) && await Users.AnyAsync(n => n.Id != user.Id && n.PhoneNumber == user.PhoneNumber))
            {
                return AbpIdentityResult.Failed($"手机号 '{user.PhoneNumber}' 已被占用");
            }
            return await base.UpdateAsync(user);
        }
    }
}
