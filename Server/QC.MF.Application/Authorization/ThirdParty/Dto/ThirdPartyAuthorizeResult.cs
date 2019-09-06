namespace QC.MF.Authorization.ThirdParty.Dto
{
    /// <summary>
    /// 第三方用户认证信息
    /// </summary>
    public class ThirdPartyAuthorizeResult
    {
        /// <summary>
        /// 是否登录成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 是否需要创建新用户
        /// </summary>
        public bool RequireCreateNewUser { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 第三方用户信息
        /// </summary>
        public ThirdPartyUserOutput ThirdPartyUser { get; set; }
    }
}
