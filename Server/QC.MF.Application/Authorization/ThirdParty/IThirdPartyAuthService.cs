using QC.MF.Authorization.ThirdParty.Dto;

namespace QC.MF.Authorization.ThirdParty
{
    /// <summary>
    /// 第三方服务
    /// </summary>
    public interface IThirdPartyAuthService
    {
        /// <summary>
        /// 获取跳转第三方登录链接地址
        /// </summary>
        /// <returns></returns>
        string GetAuthRedirectUrl();

        /// <summary>
        /// 调用第三方认证
        /// </summary>
        /// <returns></returns>
        ThirdPartyAuthorizeResult Authorize(AuthorizationInput input);
        /// <summary>
        /// 获取跳转第三方绑定链接地址
        /// </summary>
        /// <returns></returns>
        string GetBindingRedirectUrl();
    }
}
