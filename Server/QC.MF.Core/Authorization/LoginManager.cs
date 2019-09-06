using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Zero.Configuration;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;
using QC.MF.Configuration;
using QC.MF.MultiTenancy;
using System.Threading.Tasks;

namespace QC.MF.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        private readonly ISettingManager _settingManager;
        private readonly IRepository<User, long> _userRepository;
        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IRepository<User, long> userRepository,
            IUserManagementConfig userManagementConfig, IIocResolver iocResolver,
            RoleManager roleManager)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  roleManager)
        {
            _settingManager = settingManager;
            _userRepository = userRepository;
        }

        public async Task CheckLoginSetting(string usernameOrEmailAddress)
        {
            var isPhoneNumberConfirmationRequiredForLogin = await _settingManager.GetSettingValueAsync<bool>(AppSettingNames.UserManagement.IsPhoneNumberConfirmationRequiredForLogin);
            var isEmailConfirmationRequiredForLogin = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
            var user = await _userRepository.FirstOrDefaultAsync(x => x.UserName == usernameOrEmailAddress || x.EmailAddress == usernameOrEmailAddress);

            if (user == null)
            {
                return;
            }

            var needPhoneNumberConfirmation = isPhoneNumberConfirmationRequiredForLogin && !user.IsPhoneNumberConfirmed;
            var needEmailConfirmation = isEmailConfirmationRequiredForLogin && !user.IsEmailConfirmed;

            if (needPhoneNumberConfirmation && needEmailConfirmation)
            {
                throw new UserFriendlyException(
                    (int)(LoginSettingVerificationResult.NeedEmailConfirmation | LoginSettingVerificationResult.NeedPhoneNumberConfirmation),
                    "登录失败",
                    "需要激活手机号和邮箱才能登录。");
            }
            if (needPhoneNumberConfirmation)
            {
                throw new UserFriendlyException(
                    (int)(LoginSettingVerificationResult.NeedPhoneNumberConfirmation),
                    "登录失败",
                    "需要激活手机号才能登录。");
            }
            if (needEmailConfirmation)
            {
                throw new UserFriendlyException(
                    (int)(LoginSettingVerificationResult.NeedEmailConfirmation),
                    "登录失败",
                    "需要激活邮箱才能登录。");
            }

        }
    }
}
