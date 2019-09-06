namespace QC.MF.Authorization.Accounts.Dto
{
    /// <summary>
    /// 绑定账号参数
    /// </summary>
    public class BindingThirdPartyInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 认证令牌
        /// </summary>
        public string Token { get; set; }
    }
}
