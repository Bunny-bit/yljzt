using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Web.Models;
using Abp.WebApi.Controllers;
using Abp.WebApi.Extensions;
using QC.MF.Api.Models;
using QC.MF.Authorization;
using QC.MF.Authorization.Users;
using QC.MF.MultiTenancy;
using QC.MF.Users;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using QC.MF.Captcha;
using QC.MF.Configuration;
using Abp.Configuration;

namespace QC.MF.Api.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    public class AccountController : AbpApiController
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        private readonly LogInManager _logInManager;
        private readonly ICacheManager _cacheManager;
        private readonly ICaptchaManager _captchaManager;

        static AccountController()
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logInManager"></param>
        /// <param name="cacheManager"></param>
        public AccountController(LogInManager logInManager, ICacheManager cacheManager, ICaptchaManager captchaManager)
        {
            _logInManager = logInManager;
            _cacheManager = cacheManager;
            _captchaManager = captchaManager;
            LocalizationSourceName = MFConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 登录认证
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResponse> Authenticate(LoginModel loginModel)
        {
            CheckModelState();
            //_captchaManager.CheckCaptcha(loginModel.Captcha);

            var loginResult = await GetLoginResultAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password,
                "Default"
                );

            var ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(loginModel.RememberMe ? TimeSpan.FromDays(3) : TimeSpan.FromMinutes(30));

            return new AjaxResponse(OAuthBearerOptions.AccessTokenFormat.Protect(ticket));
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);


            if (SettingManager.GetSettingValueForApplication<bool>(AppSettingNames.UserManagement.IsPhoneNumberConfirmationRequiredForLogin)
                 && !loginResult.User.IsPhoneNumberConfirmed)
            {
                throw new UserFriendlyException("登录失败", "没有验证手机号！");
            }

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException("登录失败", "用户名或密码错误！");
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException("登录失败", "租户名称错误！");
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException("登录失败", "租户被禁用！");
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException("登录失败", "用户被禁用！");
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException("登录失败", "没有验证邮箱！"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException("登录失败");
            }
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Invalid request!");
            }
        }
    }
}
