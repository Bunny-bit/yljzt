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
using QC.MF.SMSs;
using Microsoft.AspNet.Identity;

namespace QC.MF.Api.Controllers
{
    /// <summary>
    /// 手机短信登录
    /// </summary>
    public class SMSLoginController : AbpApiController
    {

        private readonly LogInManager _logInManager;
        private readonly ICacheManager _cacheManager;
        private readonly ICaptchaManager _captchaManager;
        private readonly ISMSManager _smsManager;
        private readonly UserManager _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logInManager"></param>
        /// <param name="cacheManager"></param>
        public SMSLoginController(
            LogInManager logInManager, 
            ICacheManager cacheManager,
            ICaptchaManager captchaManager,
            UserManager userManager,
            ISMSManager smsManager
            )
        {
            _logInManager = logInManager;
            _cacheManager = cacheManager;
            _captchaManager = captchaManager;
            LocalizationSourceName = MFConsts.LocalizationSourceName;
            _smsManager = smsManager;
            _userManager = userManager;
        }


        /// <summary>
        /// 手机短信登录认证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResponse> Authenticate(LoginForSmsCode input)
        {
            _smsManager.ValidateVerificationCode(input.PhoneNumber, input.Code);
            var user= _userManager.Users.Single(x => x.PhoneNumber == input.PhoneNumber);


            var identity = await _userManager.CreateIdentityAsync(user, "SmsLogin");
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());

            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add( TimeSpan.FromDays(3) );

            return new AjaxResponse(AccountController. OAuthBearerOptions.AccessTokenFormat.Protect(ticket));
        }
        
    }
}
