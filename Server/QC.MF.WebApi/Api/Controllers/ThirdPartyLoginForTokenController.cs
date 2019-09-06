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
using QC.MF.Authorization.ThirdParty;
using QC.MF.Authorization.ThirdParty.Dto;

namespace QC.MF.Api.Controllers
{
    /// <summary>
    /// 第三方登录   返回token
    /// </summary>
    public class ThirdPartyLoginForTokenController : AbpApiController
    {

        private readonly LogInManager _logInManager;
        private readonly ICacheManager _cacheManager;
        private readonly ICaptchaManager _captchaManager;
        private readonly ISMSManager _smsManager;
        private readonly UserManager _userManager;

        private readonly QQAuthService _qqAuthService;
        private readonly WeixinAuthService _weixinAuthService;
        private readonly WeiboAuthService _weiboAuthService;
        private readonly AlipayAuthService _alipayAuthService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ThirdPartyLoginForTokenController(
            LogInManager logInManager,
            ICacheManager cacheManager,
            ICaptchaManager captchaManager,
            UserManager userManager,
            ISMSManager smsManager,
            QQAuthService qqAuthService,
            WeixinAuthService weixinAuthService,
            WeiboAuthService weiboAuthService,
            AlipayAuthService alipayAuthService
            )
        {
            _logInManager = logInManager;
            _cacheManager = cacheManager;
            _captchaManager = captchaManager;
            LocalizationSourceName = MFConsts.LocalizationSourceName;
            _smsManager = smsManager;
            _userManager = userManager;
            _qqAuthService = qqAuthService;
            _weixinAuthService = weixinAuthService;
            _weiboAuthService = weiboAuthService;
            _alipayAuthService = alipayAuthService;
        }


        /// <summary>
        /// 第三方登录   返回token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResponse> Authenticate(ThirdPartyLoginModel input)
        {
            IThirdPartyAuthService authService;
            switch (input.ThirdParty)
            {
                case ThirdParty.QQ:
                    authService = _qqAuthService;
                    break;
                case ThirdParty.Weixin:
                    authService = _weixinAuthService;
                    break;
                case ThirdParty.Weibo:
                    authService = _weiboAuthService;
                    break;
                case ThirdParty.Alipay:
                    authService = _alipayAuthService;
                    break;
                default:
                    throw new UserFriendlyException("不支持您所选的登录平台");
            }
            //var codeCache = _cacheManager.GetCache("ThirdPartyAuthCodes");
            //var codeStatus = codeCache.GetOrDefault(input.Code);
            //if (codeStatus != null)
            //{
            //    throw new UserFriendlyException("认证信息已失效，请您重试第三方登录认证");
            //}

            //codeCache.Set(input.Code, input.Code, TimeSpan.FromMinutes(5));

            var authorizeResult = authService.Authorize(new AuthorizationInput { Code = input.Code });

            if (authorizeResult.Success)
            {
                var user = await _userManager.FindByIdAsync(authorizeResult.ThirdPartyUser.UserId);



                var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());

                var currentUtc = new SystemClock().UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(3));

                return new AjaxResponse(AccountController.OAuthBearerOptions.AccessTokenFormat.Protect(ticket));
            }
            return new AjaxResponse(authorizeResult);
        }


    }

}

