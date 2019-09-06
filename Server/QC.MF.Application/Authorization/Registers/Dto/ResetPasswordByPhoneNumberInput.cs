namespace QC.MF.Authorization.Registers.Dto
{
    /// <summary>
    /// 通过手机号找回密码
    /// </summary>
    public class ResetPasswordByPhoneNumberInput
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Code { get; set; }
    }
}
