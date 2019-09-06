using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Exceptions;
using Abp.Extensions;
using Abp.Localization;
using Abp.Logging;
using Abp.MultiTenancy;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Models;
using Castle.Core.Logging;
using QC.MF.Authorization;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;
using QC.MF.MultiTenancy;
using QC.MF.Sessions;
using QC.MF.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using QC.MF.Captcha;
using Abp.Configuration;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using QC.MF.Authorization.Accounts.Dto;
using QC.MF.Authorization.ThirdParty;
using QC.MF.Authorization.ThirdParty.Dto;
using QC.MF.Configuration;
using QC.MF.Users.Dto;
using QC.MF.Authorization.Dto;
using Microsoft.Extensions.Internal;

namespace QC.MF.Web.Controllers
{
    public class AccountController : MFControllerBase
    {
        public ILogger Logger { get; set; }
        public IEventBus EventBus { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly LogInManager _logInManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly ILanguageManager _languageManager;
        private readonly ITenantCache _tenantCache;
        private readonly ICaptchaManager _captchaManager;
        private readonly ISettingManager _settingManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ICacheManager _cacheManager;


        private readonly QQAuthService _qqAuthService;
        private readonly WeixinAuthService _weixinAuthService;
        private readonly WeiboAuthService _weiboAuthService;
        private readonly AlipayAuthService _alipayAuthService;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUnitOfWorkManager unitOfWorkManager,
            IMultiTenancyConfig multiTenancyConfig,
            LogInManager logInManager,
            ISessionAppService sessionAppService,
            ILanguageManager languageManager,
            ITenantCache tenantCache,
            ICaptchaManager captchaManager,
            ISettingManager settingManager, 
            QQAuthService qqAuthService, 
            WeixinAuthService weixinAuthService, 
            WeiboAuthService weiboAuthService, 
            AlipayAuthService alipayAuthService, 
            UserRegistrationManager userRegistrationManager, ICacheManager cacheManager)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkManager = unitOfWorkManager;
            _multiTenancyConfig = multiTenancyConfig;
            _logInManager = logInManager;
            _sessionAppService = sessionAppService;
            _languageManager = languageManager;
            _tenantCache = tenantCache;
            _captchaManager = captchaManager;
            _settingManager = settingManager;
            _qqAuthService = qqAuthService;
            _weixinAuthService = weixinAuthService;
            _weiboAuthService = weiboAuthService;
            _alipayAuthService = alipayAuthService;
            _userRegistrationManager = userRegistrationManager;
            _cacheManager = cacheManager;

            Logger = NullLogger.Instance;
            EventBus = NullEventBus.Instance;
        }

        #region Login / Logout
        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            try
            {
                CheckModelState();
                _captchaManager.CheckCaptcha(loginModel.Captcha);
                await _logInManager.CheckLoginSetting(loginModel.UsernameOrEmailAddress);

                var loginResult = await GetLoginResultAsync(
                    loginModel.UsernameOrEmailAddress,
                    loginModel.Password,
                    "Default" //GetTenancyNameOrNull()
                );

                await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = Request.ApplicationPath;
                }

                if (!string.IsNullOrWhiteSpace(returnUrlHash))
                {
                    returnUrl = returnUrl + returnUrlHash;
                }

                return Json(new AjaxResponse(new { ShouldChangePasswordOnNextLogin = loginResult.User.ShouldChangePasswordOnNextLogin }) { TargetUrl = returnUrl });
            }
            catch (Exception ex)
            {
                LogHelper.LogException(Logger, ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                EventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(ex)));
            }
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            //if (_settingManager.GetSettingValueForApplication<bool>(AppSettingNames.UserManagement.IsPhoneNumberConfirmationRequiredForLogin)
            //     && !loginResult.User.IsPhoneNumberConfirmed)
            //{
            //    throw new UserFriendlyException(L("LoginFailed"), "没有验证手机号");
            //}

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        /// <summary>
        /// 获取第三方登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DisableAuditing]
        public JsonResult ThirdPartyList()
        {
            try
            {
                var result = new List<ThirdPartyModel>();
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.QQ.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.QQ.ToString(),
                        ThirdPartyName = "QQ",
                        AuthUrl = _qqAuthService.GetAuthRedirectUrl(),
                        IconUrl = "/Images/qq.png"
                    });
                }
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.WeixinOpen.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.Weixin.ToString(),
                        ThirdPartyName = "微信",
                        AuthUrl = _weixinAuthService.GetAuthRedirectUrl(),
                        IconUrl = "/Images/wechat.png"
                    });
                }
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.Weibo.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.Weibo.ToString(),
                        ThirdPartyName = "微博",
                        AuthUrl = _weiboAuthService.GetAuthRedirectUrl(),
                        IconUrl = "/Images/weibo.png"
                    });
                }
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.Alipay.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.Alipay.ToString(),
                        ThirdPartyName = "支付宝",
                        AuthUrl = _alipayAuthService.GetAuthRedirectUrl(),
                        IconUrl = "/Images/alipay.png"
                    });
                }
                return Json(new AjaxResponse(result));
            }
            catch (Exception ex)
            {
                LogHelper.LogException(Logger, ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                EventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(ex)));
            }
        }


        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> ThirdPartyLogin(ThirdPartyLoginModel input)
        {
            try
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
                var codeCache = _cacheManager.GetCache("ThirdPartyAuthCodes");
                var codeStatus = codeCache.GetOrDefault(input.Code);
                if (codeStatus != null)
                {
                    throw new UserFriendlyException("认证信息已失效，请您重试第三方登录认证");
                }

                codeCache.Set(input.Code, input.Code, TimeSpan.FromMinutes(5));

                var authorizeResult = authService.Authorize(new AuthorizationInput { Code = input.Code });

                if (authorizeResult.Success)
                {
                    var user = await _userManager.FindByIdAsync(authorizeResult.ThirdPartyUser.UserId);

                    //if (_settingManager.GetSettingValueForApplication<bool>(AppSettingNames.UserManagement.IsPhoneNumberConfirmationRequiredForLogin)
                    //    && !user.IsPhoneNumberConfirmed)
                    //{
                    //    throw new UserFriendlyException(L("LoginFailed"), "没有验证手机号");
                    //}

                    //if (_settingManager.GetSettingValueForApplication<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin)
                    //    && !user.IsEmailConfirmed)
                    //{
                    //    throw new UserFriendlyException(L("LoginFailed"), "没有验证邮箱地址");
                    //}
                    await SignInAsync(user);
                }
                return Json(new AjaxResponse(authorizeResult));
            }
            catch (Exception ex)
            {
                LogHelper.LogException(Logger, ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                EventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(ex)));
            }
        }

        /// <inheritdoc />
        public async Task<JsonResult> BindingThirdParty(BindingThirdPartyInput input)
        {
            try
            {
                var result = await _logInManager.LoginAsync(input.UserName, input.Password);
                if (result.Result != AbpLoginResultType.Success)
                {
                    throw new UserFriendlyException("用户名或密码输入错误");
                }
                if (string.IsNullOrEmpty(input.Token))
                {
                    throw new UserFriendlyException("第三方认证令牌有误或者已失效，请重新绑定");
                }
                await _userRegistrationManager.BindingThirdPartyAsync(input.Token, result.User);
                await SignInAsync(result.User);
                return Json(new AjaxResponse());
            }
            catch (Exception ex)
            {
                LogHelper.LogException(Logger, ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                EventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(ex)));
            }
        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "UserEmailIsNotConfirmedAndCanNotLogin");
                case AbpLoginResultType.LockedOut:
                    return new UserFriendlyException(L("LoginFailed"), L("UserLockedOutMessage"));
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }

        public JsonResult Logout()
        {
            AuthenticationManager.SignOut();
            return Json(new AjaxResponse(true));
        }

        #endregion

        #region Register
        private bool IsSelfRegistrationEnabled()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return false; //No registration enabled for host users!
            }

            return true;
        }
        


        /// <summary>
        /// 获取第三方登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> GetBindingThirdPartyList()
        {
            try
            {
                if (!AuthenticationManager.User.Identity.IsAuthenticated)
                {
                    throw new UserFriendlyException("当前用户没有登录");
                }
                var userid = AuthenticationManager.User.Identity.GetUserId<long>();
                var bindingUsers = await _userRegistrationManager.GetBindingUsersAsync(userid);
                var result = new List<ThirdPartyModel>();
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.QQ.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.QQ.ToString(),
                        ThirdPartyName = "QQ",
                        AuthUrl = _qqAuthService.GetBindingRedirectUrl(),
                        IconUrl = "/Images/qq.png",
                        IsBinding = bindingUsers.Exists(u=>u.ThirdParty == ThirdParty.QQ.ToString())
                    });
                }
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.WeixinOpen.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.Weixin.ToString(),
                        ThirdPartyName = "微信",
                        AuthUrl = _weixinAuthService.GetBindingRedirectUrl(),
                        IconUrl = "/Images/wechat.png",
                        IsBinding = bindingUsers.Exists(u => u.ThirdParty == ThirdParty.Weixin.ToString())
                    });
                }
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.Weibo.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.Weibo.ToString(),
                        ThirdPartyName = "微博",
                        AuthUrl = _weiboAuthService.GetBindingRedirectUrl(),
                        IconUrl = "/Images/weibo.png",
                        IsBinding = bindingUsers.Exists(u => u.ThirdParty == ThirdParty.Weibo.ToString())
                    });
                }
                if (SettingManager.GetSettingValue<bool>(AppSettingNames.OAuth.Alipay.IsEnabled))
                {
                    result.Add(new ThirdPartyModel
                    {
                        ThirdParty = ThirdParty.Alipay.ToString(),
                        ThirdPartyName = "支付宝",
                        AuthUrl = _alipayAuthService.GetBindingRedirectUrl(),
                        IconUrl = "/Images/alipay.png",
                        IsBinding = bindingUsers.Exists(u => u.ThirdParty == ThirdParty.Alipay.ToString())
                    });
                }
                return Json(new AjaxResponse(result));
            }
            catch (Exception ex)
            {
                LogHelper.LogException(Logger, ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                EventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(ex)));
            }
        }
        /// <inheritdoc />
        public async Task<JsonResult> LoginUserBindingThirdParty(ThirdPartyLoginModel input)
        {
            try
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
                var authorizeResult = authService.Authorize(new AuthorizationInput { Code = input.Code });

                if (!AuthenticationManager.User.Identity.IsAuthenticated)
                {
                    throw new UserFriendlyException("当前用户没有登录");
                }
                var userid = AuthenticationManager.User.Identity.GetUserId<long>();
                var user = await _userManager.FindByIdAsync(userid);
                await _userRegistrationManager.BindingThirdPartyAsync(authorizeResult.Token, user);
                return Json(new AjaxResponse(new { success = true, message = "", platform = input.ThirdParty.GetDescription() }));
            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse(new { success=false, message=ex.Message, platform= input.ThirdParty.GetDescription() }));
            }
        }

        /// <inheritdoc />
        public async Task<JsonResult> LoginUserUnbindingThirdParty(UnbindingThirdPartyModel input)
        {
            try
            {
                if (!AuthenticationManager.User.Identity.IsAuthenticated)
                {
                    throw new UserFriendlyException("当前用户没有登录");
                }
                var userid = AuthenticationManager.User.Identity.GetUserId<long>();
                var user = await _userManager.FindByIdAsync(userid);
                await _userRegistrationManager.UnbindingThirdPartyAsync(input.ThirdParty.ToString(), user);
                return Json(new AjaxResponse());
            }
            catch (Exception ex)
            {
                LogHelper.LogException(Logger, ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                EventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(ex)));
            }
        }

        #endregion

        #region External Login

        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .ToList();
        }

        private static bool TryExtractNameAndSurnameFromClaims(List<Claim> claims, ref string name, ref string surname)
        {
            string foundName = null;
            string foundSurname = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                foundName = givennameClaim.Value;
            }

            var surnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            if (surnameClaim != null && !surnameClaim.Value.IsNullOrEmpty())
            {
                foundSurname = surnameClaim.Value;
            }

            if (foundName == null || foundSurname == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    var nameSurName = nameClaim.Value;
                    if (!nameSurName.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = nameSurName.LastIndexOf(' ');
                        if (lastSpaceIndex < 1 || lastSpaceIndex > (nameSurName.Length - 2))
                        {
                            foundName = foundSurname = nameSurName;
                        }
                        else
                        {
                            foundName = nameSurName.Substring(0, lastSpaceIndex);
                            foundSurname = nameSurName.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            if (!foundName.IsNullOrEmpty())
            {
                name = foundName;
            }

            if (!foundSurname.IsNullOrEmpty())
            {
                surname = foundSurname;
            }

            return foundName != null && foundSurname != null;
        }

        #endregion

        #region Common private methods

        private async Task<Tenant> GetActiveTenantAsync(string tenancyName)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIsNotActive", tenancyName));
            }

            return tenant;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        #endregion
    }
}
