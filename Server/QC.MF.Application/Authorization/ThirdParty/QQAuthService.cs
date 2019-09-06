
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using Newtonsoft.Json;
using QC.MF.Authorization.ThirdParty.Dto;
using QC.MF.Authorization.Users;
using QC.MF.Configuration;

namespace QC.MF.Authorization.ThirdParty
{
    /// <summary>
    /// 
    /// </summary>
    public class QQAuthService : MFDomainServiceBase, IThirdPartyAuthService
    {
        private string AppId => SettingManager.GetSettingValue(AppSettingNames.OAuth.QQ.AppID);
        private string AppKey => SettingManager.GetSettingValue(AppSettingNames.OAuth.QQ.AppKey);
        private string RedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl)+"?type=QQ";
        private string BindingRedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl) + "?type=QQ|binding";
        private const string Scope = "get_user_info";

        private readonly UserManager _userManager;
        private readonly IRepository<ThirdPartyUser, long> _thirdPartyUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="thirdPartyUserRepository"></param>
        public QQAuthService(
            UserManager userManager, 
            IRepository<ThirdPartyUser, long> thirdPartyUserRepository)
        {
            _userManager = userManager;
            _thirdPartyUserRepository = thirdPartyUserRepository;
        }

        /// <inheritdoc />
        public string GetAuthRedirectUrl()
        {
            return "https://graph.qq.com/oauth2.0/authorize" +
                   $"?response_type=code&client_id={AppId}&redirect_uri={RedirectUri}&scope={Scope}";
        }

        private string ParseAccessToken(string tokenResult)
        {
            if (string.IsNullOrEmpty(tokenResult))
            {
                throw new UserFriendlyException("认证失败，请重试");
            }
            //access_token=D7773905D283C492E94CF39584AC4F92&expires_in=7776000&refresh_token=B4740C8B6914FEFC2CC135A9B3DC0406
            if (!Regex.IsMatch(tokenResult, "access_token=([A-Z0-9]*)"))
            {
                throw new UserFriendlyException("认证失败，请重试");
            }
            return Regex.Match(tokenResult, "access_token=([A-Z0-9]*)").Groups[1].Value;
        }

        private string ParseOpenId(string openIdResult)
        {
            //callback( { "client_id":"101434716","openid":"BCD6CEB4F1B460D4C42662FC34C99BC9"} );
            if (string.IsNullOrEmpty(openIdResult))
            {
                throw new UserFriendlyException("认证失败，请重试");
            }
            if (!Regex.IsMatch(openIdResult, "\"openid\":\"([A-Z0-9]*)\""))
            {
                throw new UserFriendlyException("认证失败，请重试");
            }
            return Regex.Match(openIdResult, "\"openid\":\"([A-Z0-9]*)\"").Groups[1].Value;
        }

        private class UserInfo
        {
            public int ret { get; set; } 
            public string msg{ get; set; } 
            public string nickname{ get; set; } 
        
    }

        /// <inheritdoc />
        public ThirdPartyAuthorizeResult Authorize(AuthorizationInput input)
        {
            var tokenRequest = "https://graph.qq.com/oauth2.0/token" +
                      $"?grant_type=authorization_code&client_id={AppId}&client_secret={AppKey}" +
                      $"&code={input.Code}&redirect_uri={RedirectUri}";
            var tokenResult = HttpRequester.Request(tokenRequest, new HttpRequester.RequestOptions());
            var accessToken = ParseAccessToken(tokenResult);
            var openIdRequest = "https://graph.qq.com/oauth2.0/me" +
                                $"?access_token={accessToken}";
            var openIdResult = HttpRequester.Request(openIdRequest, new HttpRequester.RequestOptions());
            var openId = ParseOpenId(openIdResult);
            var thirdPartyUser = _thirdPartyUserRepository
                .GetAll()
                .FirstOrDefault(u => u.OpenId == openId);
            if (thirdPartyUser == null)
            {
                var userRequest = "https://graph.qq.com/user/get_user_info" +
                    $"?access_token={accessToken}&oauth_consumer_key={AppId}&openid={openId}";
                var userResult = HttpRequester.Request(userRequest, new HttpRequester.RequestOptions());
                var user = JsonConvert.DeserializeObject<UserInfo>(userResult);
                thirdPartyUser = new ThirdPartyUser
                {
                    OpenId = openId,
                    AccessToken = accessToken,
                    NickName = user.nickname,
                    ThirdParty = "QQ"
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
                Token = $"OpenId={openId}&date={DateTime.Now:yyyy-MM-dd HH:mm:ss}&type=QQ".EncryptQueryString(),
                Success = thirdPartyUser.UserId > 0,
                RequireCreateNewUser = thirdPartyUser.UserId == 0
            };
        }

        /// <inheritdoc />
        public string GetBindingRedirectUrl()
        {
            return "https://graph.qq.com/oauth2.0/authorize" +
                   $"?response_type=code&client_id={AppId}&redirect_uri={BindingRedirectUri}&scope={Scope}";
        }
    }
}
