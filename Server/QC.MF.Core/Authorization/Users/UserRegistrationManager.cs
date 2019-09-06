using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Services;
using Abp.IdentityFramework;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Session;
using Abp.UI;
using QC.MF.Authorization.Roles;
using QC.MF.MultiTenancy;
using Microsoft.AspNet.Identity;
using QC.MF.Configuration;
using QC.MF.Notifications;
using System.Data.Entity;
using Abp.Domain.Repositories;
using QC.MF.SMSs;
using Abp.Runtime.Caching;
using QC.MF.Authorization.ThirdParty;
using QC.MF.Captcha;
using QC.MF.VerificationCodes;

namespace QC.MF.Authorization.Users
{
    public class UserRegistrationManager : MFDomainServiceBase
    {
        public IAbpSession AbpSession { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly ISmtpEmailSender _emailSender;
        private readonly IAppNotifier _appNotifier;
        private readonly ISMSManager _smsManager;
        private readonly ICacheManager _cacheManager;
        private readonly ICaptchaManager _captchaManager;

        private readonly IRepository<ThirdPartyUser, long> _thirdPartyUserRepository;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            ISmtpEmailSender emailSender,
            IAppNotifier appNotifier,
            ISMSManager smsManager,
            ICacheManager cacheManager,
            ICaptchaManager captchaManager, 
            IRepository<ThirdPartyUser, long> thirdPartyUserRepository)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _appNotifier = appNotifier;
            _smsManager = smsManager;
            _cacheManager = cacheManager;
            _captchaManager = captchaManager;
            _thirdPartyUserRepository = thirdPartyUserRepository;

            AbpSession = NullAbpSession.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed)
        {
            CheckForTenant();
            CheckIsRegisterEnabled();
            var tenant = await GetActiveTenantAsync();

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = true,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                IsPhoneNumberConfirmed = false,
                Roles = new List<UserRole>(),
                Password = new PasswordHasher().HashPassword(plainPassword)
            };


            foreach (var defaultRole in _roleManager.Roles.Where(r => r.IsDefault).ToList())
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            CheckErrors(await _userManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync();

            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }

        /// <summary>
        /// 通过手机号注册
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="userName"></param>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        public async Task<User> RegisterAsync(string name, string surname, string phoneNumber, string userName, string plainPassword)
        {
            CheckForTenant();
            CheckIsRegisterEnabled();

            if ((await _userManager.FindUserByPhoneNumberAsync(phoneNumber)) != null)
            {
                throw new UserFriendlyException("当前手机号已被注册，注册失败");
            }

            var tenant = await GetActiveTenantAsync();

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                PhoneNumber = phoneNumber,
                IsActive = true,
                UserName = userName,
                EmailAddress = "",
                IsEmailConfirmed = false,
                IsPhoneNumberConfirmed = true,
                Roles = new List<UserRole>(),
                Password = new PasswordHasher().HashPassword(plainPassword)
            };


            foreach (var defaultRole in _roleManager.Roles.Where(r => r.IsDefault).ToList())
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            CheckErrors(await _userManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync();

            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }

        public async Task ResetPasswordByPhoneNumberAsync(string phoneNumber, string password)
        {
            var user = await _userManager.FindUserByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                throw new UserFriendlyException("找不到手机号关联用户");
            }
            user.Password = new PasswordHasher().HashPassword(password);
            await CurrentUnitOfWork.SaveChangesAsync();
        }


        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private void CheckIsRegisterEnabled()
        {
            var value = SettingManager.GetSettingValue<bool>(AppSettingNames.UserManagement.AllowSelfRegistration);
            if (!value)
            {
                throw new UserFriendlyException("当前系统未开放注册，请联系系统管理员为您创建账号");
            }
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }



        public async Task SendConfirmEmailCodeAsync(string email, string captcha)
        {
            _captchaManager.CheckCaptcha(captcha);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserFriendlyException("找不到此邮箱关联的用户");
            }
            if (user.IsActive && user.IsEmailConfirmed)
            {
                throw new UserFriendlyException("您的邮箱已处于激活状态，直接登录即可");
            }
            string code = VerificationCode.GetRandomCode();

            _cacheManager.GetCache("EmailCode").Set(email, code, new TimeSpan(0, 10, 0));

            var secretkey = user.Id + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "|" + Guid.NewGuid().ToString("N");
            secretkey = secretkey.EncryptQueryString();
            var url = await SettingManager.GetSettingValueAsync(AppSettingNames.SiteUrl) + $"/#/confirm?key={secretkey}";
            var subject = "验证码";
            var body = $"您好，您的验证码为：{code}";
            await _emailSender.SendAsync(email, subject, body);
        }

        public async Task ConfirmEmailByCodeAsync(string email, string code)
        {
            if (_cacheManager.GetCache("EmailCode").GetOrDefault(email).ToString() != code)
            {
                throw new UserFriendlyException("验证码输入错误");
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(n => n.EmailAddress == email);
            if (user == null)
            {
                throw new UserFriendlyException("找不到此邮箱关联的用户");
            }
            if (user.IsEmailConfirmed)
            {
                throw new UserFriendlyException("您已验证过邮箱！");
            }
            user.IsActive = true;
            user.IsEmailConfirmed = true;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task SendConfirmPhoneNumberByCodeAsync(string phoneNumber, string captcha)
        {
            _captchaManager.CheckCaptcha(captcha);
            var user = await _userManager.Users.FirstOrDefaultAsync(n => n.PhoneNumber == phoneNumber);
            if (user == null)
            {
                throw new UserFriendlyException("找不到此手机号关联的用户");
            }
            if (user.IsPhoneNumberConfirmed)
            {
                throw new UserFriendlyException("您已验证过手机号！");
            }
            await _smsManager.SendVerificationCode(phoneNumber);
        }
        public async Task ConfirmPhoneNumberByCodeAsync(string phoneNumber, string code)
        {
            _smsManager.ValidateVerificationCode(phoneNumber, code);
            var user = await _userManager.Users.FirstOrDefaultAsync(n => n.PhoneNumber == phoneNumber);
            if (user == null)
            {
                throw new UserFriendlyException("找不到此手机号关联的用户！");
            }
            if (user.IsPhoneNumberConfirmed)
            {
                throw new UserFriendlyException("您已验证过手机号！");
            }
            user.IsActive = true;
            user.IsPhoneNumberConfirmed = true;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task BindingThirdPartyAsync(string token, User user)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            if (user == null)
            {
                return;
            }
            try
            {
                var thirdPartyInfo = token.DecryptQueryString();
                var strs = thirdPartyInfo.ToStringList('&');
                var openid = strs[0].ToStringList('=')[1];
                var time = strs[1].ToStringList('=')[1].ToDatetime();
                var type = strs[2].ToStringList('=')[1];

                var thirdPartyUser = await _thirdPartyUserRepository.GetAll().FirstOrDefaultAsync(u => u.OpenId == openid);
                if (thirdPartyUser == null)
                {
                    throw new UserFriendlyException("第三方令牌无效，请重试");
                }
                if (thirdPartyUser.UserId > 0)
                {
                    throw new UserFriendlyException("第三方账号已绑定其他用户，直接登录即可");
                }
                thirdPartyUser.UserId = user.Id;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw new UserFriendlyException("第三方令牌无效，请重试");
            }
        }

        public async Task UnbindingThirdPartyAsync(string thirdParty, User user)
        {
            if (string.IsNullOrEmpty(thirdParty))
            {
                throw new UserFriendlyException("解绑失败，未指定解绑平台");
            }
            if (user == null)
            {
                throw new UserFriendlyException("解绑失败，用户未登录或登录会话已过期");
            }
            var thirdPartyUser = await _thirdPartyUserRepository.GetAll()
                .FirstOrDefaultAsync(u => u.UserId == user.Id && u.ThirdParty== thirdParty);
            if (thirdPartyUser == null)
            {
                throw new UserFriendlyException("解绑失败，未能查询到绑定信息");
            }
            thirdPartyUser.UserId = 0;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<List<ThirdPartyUser>> GetBindingUsersAsync(long userId)
        {
            if (userId == 0)
            {
                return new List<ThirdPartyUser>();
            }
            return await _thirdPartyUserRepository.GetAll().Where(g => g.UserId == userId).ToListAsync();
        }
        

        public async Task SendEmailCodeAsync(string email, string captcha)
        {
            _captchaManager.CheckCaptcha(captcha);
            string code = VerificationCode.GetRandomCode();

            _cacheManager.GetCache("EmailCode").Set(email, code, new TimeSpan(0, 10, 0));
            
            var subject = "验证码";
            var body = $"您好，您的验证码为：{code}";
            await _emailSender.SendAsync(email, subject, body);
        }

        public void ValidateEmailCode(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(code)|| _cacheManager.GetCache("EmailCode").GetOrDefault(email)?.ToString() != code)
            {
                throw new UserFriendlyException("验证码输入错误");
            }
            _cacheManager.GetCache("EmailCode").Remove(email);
        }
    }
}
