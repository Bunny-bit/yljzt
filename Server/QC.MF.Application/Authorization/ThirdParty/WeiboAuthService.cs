using System;
using System.Linq;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Newtonsoft.Json;
using QC.MF.Authorization.ThirdParty.Dto;
using QC.MF.Configuration;

namespace QC.MF.Authorization.ThirdParty
{
    /// <summary>
    /// 微博登录服务
    /// </summary>
    public class WeiboAuthService : MFDomainServiceBase, IThirdPartyAuthService
    {

        private string AppId => SettingManager.GetSettingValue(AppSettingNames.OAuth.Weibo.AppID);
        private string AppSecret => SettingManager.GetSettingValue(AppSettingNames.OAuth.Weibo.AppSecret);
        private string RedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl);
        private string BindingRedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl);

        private readonly IRepository<ThirdPartyUser, long> _thirdPartyUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="thirdPartyUserRepository"></param>
        public WeiboAuthService(IRepository<ThirdPartyUser, long> thirdPartyUserRepository)
        {
            _thirdPartyUserRepository = thirdPartyUserRepository;
        }

        /// <inheritdoc />
        public string GetAuthRedirectUrl()
        {
            return "https://api.weibo.com/oauth2/authorize" +
                $"?client_id={AppId}&response_type=code&redirect_uri={RedirectUri}&state=Weibo";
        }

        private class AuthorizeResult
        {
            public string access_token { get; set; }
            public int remind_in { get; set; }
            public int expires_in { get; set; }
        }

        private class UserInfo
        {
            public string id { get; set; }
            public string screen_name { get; set; }
            public string name { get; set; }
            
        }

        /// <inheritdoc />
        public ThirdPartyAuthorizeResult Authorize(AuthorizationInput input)
        {
            var tokenRequest = "https://api.weibo.com/oauth2/access_token" +
                $"?client_id={AppId}&client_secret={AppSecret}&grant_type=authorization_code" +
                $"&redirect_uri={RedirectUri}&code={input.Code}";
            var tokenResult = HttpRequester.Request(tokenRequest, 
                new HttpRequester.RequestOptions
                {
                    Method = HttpRequester.HttpMethod.Post
                });
            var authorizeResult = JsonConvert.DeserializeObject<AuthorizeResult>(tokenResult);
            var accessToken = authorizeResult.access_token;

            var uidRequest = "https://api.weibo.com/2/account/get_uid.json" +
                $"?access_token={accessToken}";
            var uidResult = HttpRequester.Request(uidRequest, new HttpRequester.RequestOptions());
            var uid = (string)JsonConvert.DeserializeObject<dynamic>(uidResult).uid;

            var thirdPartyUser = _thirdPartyUserRepository
                .GetAll()
                .FirstOrDefault(u => u.OpenId == uid);
            if (thirdPartyUser == null)
            {
                var userRequest = "https://api.weibo.com/2/users/show.json"
                        + $"?access_token={accessToken}&uid={uid}";

                var userResult = HttpRequester.Request(userRequest, new HttpRequester.RequestOptions());
                var user = JsonConvert.DeserializeObject<UserInfo>(userResult);
                thirdPartyUser = new ThirdPartyUser
                {
                    OpenId = uid,
                    AccessToken = accessToken,
                    Name = user.name,
                    NickName = user.screen_name,
                    ThirdParty = "Weibo"
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
                Token = $"OpenId={uid}&date={DateTime.Now:yyyy-MM-dd HH:mm:ss}&type=Weibo".EncryptQueryString(),
                Success = thirdPartyUser.UserId > 0,
                RequireCreateNewUser = thirdPartyUser.UserId == 0
            };
        }

        public string GetBindingRedirectUrl()
        {
            return "https://api.weibo.com/oauth2/authorize" +
                   $"?client_id={AppId}&response_type=code&redirect_uri={BindingRedirectUri}&state=Weibo|binding";
        }
    }
}
