namespace QC.MF.Authorization.ThirdParty.Dto
{
    /// <summary>
    /// 第三方认证请求参数
    /// </summary>
    public class AuthorizationInput
    {
        /// <summary>
        /// 认证码
        /// </summary>
        public string Code { get; set; }
    }
}
