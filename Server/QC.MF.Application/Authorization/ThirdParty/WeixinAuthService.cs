using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Newtonsoft.Json;
using QC.MF.Authorization.ThirdParty.Dto;
using QC.MF.Authorization.Users;
using QC.MF.Configuration;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace QC.MF.Authorization.ThirdParty
{
    /// <summary>
    /// 微信授权服务
    /// </summary>
    public class WeixinAuthService : MFDomainServiceBase, IThirdPartyAuthService
    {
        private string AppId => SettingManager.GetSettingValue(AppSettingNames.OAuth.WeixinOpen.AppID);
        private string AppSecret => SettingManager.GetSettingValue(AppSettingNames.OAuth.WeixinOpen.AppSecret);

        private string RedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl) + "?type=Weixin";
        private string BindingRedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl) + "?type=Weixin|binding";

        private const string Scope = "snsapi_login";

        private readonly UserManager _userManager;
        private readonly IRepository<ThirdPartyUser, long> _thirdPartyUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="thirdPartyUserRepository"></param>
        public WeixinAuthService(
            UserManager userManager, 
            IRepository<ThirdPartyUser, long> thirdPartyUserRepository)
        {
            _userManager = userManager;
            _thirdPartyUserRepository = thirdPartyUserRepository;
        }

        /// <inheritdoc />
        public string GetAuthRedirectUrl()
        {
            return "https://open.weixin.qq.com/connect/qrconnect" +
                $"?appid={AppId}&redirect_uri={RedirectUri}&response_type=code" +
                $"&scope={Scope}&state={Guid.NewGuid():N}#wechat_redirect";
        }

        private class AuthorizeResult
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
        }

        private class UserInfo
        {
            public string openid { get; set; }
            public string nickname { get; set; }
            public string sex { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string headimgurl { get; set; }
            public List<string> privilege { get; set; }
            public string unionid { get; set; }

        }

        /// <inheritdoc />
        public ThirdPartyAuthorizeResult Authorize(AuthorizationInput input)
        {
            var tokenRequest = "https://api.weixin.qq.com/sns/oauth2/access_token" +
                    $"?appid={AppId}&secret={AppSecret}&code={input.Code}&grant_type=authorization_code";
            var tokenResult = HttpRequester.Request(tokenRequest, new HttpRequester.RequestOptions());
            var authorizeResult = JsonConvert.DeserializeObject<AuthorizeResult>(tokenResult);
            var accessToken = authorizeResult.access_token;
            var openId = authorizeResult.openid;
            if (openId == null)
            {
                return new ThirdPartyAuthorizeResult { Success = false };
            }
            var thirdPartyUser = _thirdPartyUserRepository
                .GetAll()
                .FirstOrDefault(u => u.OpenId == openId);
            if (thirdPartyUser == null)
            {
                var userRequest = "https://api.weixin.qq.com/sns/userinfo" +
                    $"?access_token={accessToken}&openid={openId}";
                var userResult = HttpRequester.Request(userRequest, new HttpRequester.RequestOptions());
                var user = JsonConvert.DeserializeObject<UserInfo>(userResult);
                thirdPartyUser = new ThirdPartyUser
                {
                    OpenId = openId,
                    AccessToken = accessToken,
                    Name = user.nickname,
                    NickName = user.nickname,
                    ThirdParty = "Weixin"
                };
                _thirdPartyUserRepository.Insert(thirdPartyUser);
                CurrentUnitOfWork.SaveChanges();
            }
            thirdPartyUser.AccessToken = accessToken;
            CurrentUnitOfWork.SaveChanges();
            return new ThirdPartyAuthorizeResult
            {
                ThirdPartyUser = new ThirdPartyUserOutput
                {
                    UserId = thirdPartyUser.UserId,
                    Name = thirdPartyUser.NickName,
                    NickName = thirdPartyUser.NickName
                },
                Token = $"OpenId={openId}&date={DateTime.Now:yyyy-MM-dd HH:mm:ss}&type=Weixin".EncryptQueryString(),
                Success = thirdPartyUser.UserId > 0,
                RequireCreateNewUser = thirdPartyUser.UserId == 0
            };
        }

        /// <inheritdoc />
        public string GetBindingRedirectUrl()
        {
            return "https://open.weixin.qq.com/connect/qrconnect" +
                   $"?appid={AppId}&redirect_uri={BindingRedirectUri}&response_type=code" +
                   $"&scope={Scope}&state={Guid.NewGuid():N}#wechat_redirect";
        }
    }
}
