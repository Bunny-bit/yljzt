using System;
using System.Linq;
using System.Web;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Newtonsoft.Json;
using QC.MF.Authorization.ThirdParty.Dto;
using QC.MF.Authorization.Users;
using QC.MF.Configuration;

namespace QC.MF.Authorization.ThirdParty
{
    /// <summary>
    /// 支付宝第三方认证服务
    /// </summary>
    public class AlipayAuthService : MFDomainServiceBase, IThirdPartyAuthService
    {
        private string AppId => SettingManager.GetSettingValue(AppSettingNames.OAuth.Alipay.AppID);
        private string AppPrivateKey => SettingManager.GetSettingValue(AppSettingNames.OAuth.Alipay.AppPrivateKey);
        private string AppPublicKey => SettingManager.GetSettingValue(AppSettingNames.OAuth.Alipay.AlipayPublicKey);
        private string RedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl) + "?type=Alipay";
        private string BindingRedirectUri => SettingManager.GetSettingValue(AppSettingNames.SiteUrl) + "?type=Alipay_binding";

        private readonly IRepository<ThirdPartyUser, long> _thirdPartyUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="thirdPartyUserRepository"></param>
        public AlipayAuthService(IRepository<ThirdPartyUser, long> thirdPartyUserRepository)
        {
            _thirdPartyUserRepository = thirdPartyUserRepository;
        }

        /// <inheritdoc />
        public string GetAuthRedirectUrl()
        {
            return "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm" +
                   $"?app_id={AppId}&scope=auth_base,auth_userinfo&redirect_uri={HttpUtility.UrlEncode(RedirectUri)}";
        }

        /// <inheritdoc />
        public ThirdPartyAuthorizeResult Authorize(AuthorizationInput input)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do",
                AppId, AppPrivateKey, "json", "1.0", "RSA2", AppPublicKey, "utf-8", false);
            AlipaySystemOauthTokenRequest tokenRequest = new AlipaySystemOauthTokenRequest
            {
                Code = input.Code,
                GrantType = "authorization_code"
            };
            AlipaySystemOauthTokenResponse tokenResponse = client.Execute(tokenRequest);
            if (tokenResponse.IsError)
            {
                throw new UserFriendlyException("认证失败，请重试");
            }

            var thirdPartyUser = _thirdPartyUserRepository
                .GetAll()
                .FirstOrDefault(u => u.OpenId == tokenResponse.UserId);
            if (thirdPartyUser == null)
            {
                
                AlipayUserUserinfoShareRequest userRequest = new AlipayUserUserinfoShareRequest();
                AlipayUserUserinfoShareResponse userResponse = client.Execute(userRequest, tokenResponse.AccessToken);
                if (userResponse.IsError)
                {
                    throw new UserFriendlyException("认证失败，请重试");
                }
                thirdPartyUser = new ThirdPartyUser
                {
                    OpenId = tokenResponse.UserId,
                    AccessToken = tokenResponse.AccessToken,
                    Name = userResponse.RealName,
                    NickName = userResponse.NickName,
                    ThirdParty = "Alipay"
                };
                _thirdPartyUserRepository.Insert(thirdPartyUser);
                CurrentUnitOfWork.SaveChanges();
            }
            thirdPartyUser.AccessToken = tokenResponse.UserId;
            CurrentUnitOfWork.SaveChanges();
            return new ThirdPartyAuthorizeResult
            {
                ThirdPartyUser = new ThirdPartyUserOutput
                {
                    UserId = thirdPartyUser.UserId,
                    Name = thirdPartyUser.NickName,
                    NickName = thirdPartyUser.NickName
                },
                Token = $"OpenId={tokenResponse.UserId}&date={DateTime.Now:yyyy-MM-dd HH:mm:ss}&type=Alipay".EncryptQueryString(),
                Success = thirdPartyUser.UserId>0,
                RequireCreateNewUser = thirdPartyUser.UserId == 0
            };
        }

        /// <inheritdoc />
        public string GetBindingRedirectUrl()
        {
            return "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm" +
                   $"?app_id={AppId}&scope=auth_base,auth_userinfo&redirect_uri={HttpUtility.UrlEncode(BindingRedirectUri)}";
        }
    }
}
