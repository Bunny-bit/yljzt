using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using QC.MF.Authorization.Users;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using QC.MF.Web.QRLogin.SignalR;
using Abp.Runtime.Caching;
using System.ComponentModel.DataAnnotations;

namespace QC.MF.Web.Controllers
{
    public class QRLoginController : MFControllerBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly UserManager _userManager;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private static IHubContext QRLoginHub
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<QRLoginHub>();
            }
        }

        public QRLoginController(UserManager userManager,
            ICacheManager cacheManager)
        {
            _userManager = userManager;
            _cacheManager = cacheManager;
        }

        [HttpPost]
        public JsonResult ScanQRCode(QRLoginInput input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                var errorInfo = ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("请先在手机上登录"));
                errorInfo.Code = 401;
                return Json(new AjaxResponse(errorInfo));
            }
            var findCode = _cacheManager.GetCache("QRLoginHub").GetOrDefault<string, QRCodeInfo>(input.ConnectionId);
            if (findCode == null)
            {
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("没有找到会话"))));
            }
            if (findCode.Token != input.Token)
            {
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("参数验证错误"))));
            }
            if (!findCode.IsValid())
            {
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("二维码已过期"))));
            }

            QRLoginHub.Clients.Client(input.ConnectionId).scanQRCode();
            return Json(new AjaxResponse(true));
        }

        [HttpPost]
        [AbpMvcAuthorize]
        public JsonResult ConfirmLogin(QRLoginInput input)
        {
            var findCode = _cacheManager.GetCache("QRLoginHub").GetOrDefault<string, QRCodeInfo>(input.ConnectionId);
            if (findCode == null)
            {
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("没有找到会话"))));
            }
            if (findCode.Token != input.Token)
            {
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("参数验证错误"))));
            }
            if (!findCode.IsValid())
            {
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("二维码已过期"))));
            }

            string token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            _cacheManager.GetCache("QRLoginHub").Remove(input.ConnectionId);
            _cacheManager.GetCache("QRLoginToken").Set(token, AbpSession.UserId.Value);
            QRLoginHub.Clients.Client(input.ConnectionId).confirmLogin(token);
            return Json(new AjaxResponse(true));
        }

        [HttpPost]
        public async Task<JsonResult> Login(string token)
        {
            long userId = _cacheManager.GetCache("QRLoginToken").GetOrDefault<string, long>(token);
            if (userId == default(long))
            {
                return Json(new AjaxResponse(ErrorInfoBuilder.BuildForException(new Abp.UI.UserFriendlyException("验证失败"))));
            }
            var user = await _userManager.GetUserByIdAsync(userId);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties(), identity);
            _cacheManager.GetCache("QRLoginToken").Remove(token);
            return Json(new AjaxResponse(true));
        }


        public class QRLoginInput
        {
            [Required]
            public string ConnectionId { get; set; }
            [Required]
            public Guid Token { get; set; }
        }
    }
}
