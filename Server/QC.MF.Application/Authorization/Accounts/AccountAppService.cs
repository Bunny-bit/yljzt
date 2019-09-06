using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Zero.Configuration;
using QC.MF.Authorization.Accounts.Dto;
using QC.MF.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using Newtonsoft.Json;
using QC.MF.Authorization.ThirdParty;
using QC.MF.Configuration;
using QC.MF.Security;
using QC.MF.SMSs;
using QC.MF.Users.Dto;
using QC.MF.Captcha;
using Abp.Runtime.Caching;
using Microsoft.AspNet.Identity;
using QC.MF.Authorization.Dto;

namespace QC.MF.Authorization.Accounts
{
    /// <summary>
    /// 账号服务
    /// </summary>
    public class AccountAppService : MFAppServiceBase, IAccountAppService
    {
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ISMSManager _smsManager;
        private readonly LogInManager _loginManager;
        private readonly PasswordComplexityChecker _passwordComplexityChecker;
        private readonly ICaptchaManager _captchaManager;
        private readonly ICacheManager _cacheManager;
        private readonly UserManager _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRegistrationManager"></param>
        /// <param name="smsManager"></param>
        /// <param name="loginManager"></param>
        /// <param name="passwordComplexityChecker"></param>
        /// <param name="captchaManager"></param>
        public AccountAppService(
            UserRegistrationManager userRegistrationManager,
            ISMSManager smsManager,
            LogInManager loginManager,
            PasswordComplexityChecker passwordComplexityChecker,
            ICacheManager cacheManager,
            UserManager userManager,
            ICaptchaManager captchaManager)
        {
            _userRegistrationManager = userRegistrationManager;
            _smsManager = smsManager;
            _loginManager = loginManager;
            _passwordComplexityChecker = passwordComplexityChecker;
            _cacheManager = cacheManager;
            _userManager = userManager;
            _captchaManager = captchaManager;
        }

        /// <summary>
        /// 租户是否可用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        

        /// <inheritdoc />
        public async Task BindingThirdParty(BindingThirdPartyInput input)
        {
            var result = await _loginManager.LoginAsync(input.UserName, input.Password);
            if (result.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("用户名或密码输入错误");
            }
            if (string.IsNullOrEmpty(input.Token))
            {
                throw new UserFriendlyException("第三方认证令牌有误或者已失效，请重新绑定");
            }
            await _userRegistrationManager.BindingThirdPartyAsync(input.Token, result.User);
        }


        /// <summary>
        /// 登录时发送手机证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendPhoneNumberCode(PhoneWithCaptchaInput input)
        {
            //_captchaManager.CheckCaptcha(input.Captcha);
            if ((await _userManager.FindUserByPhoneNumberAsync(input.PhoneNumber)) == null)
            {
                throw new UserFriendlyException("手机号无效");
            }
            await _smsManager.SendVerificationCode(input.PhoneNumber);
        }




    }
}
