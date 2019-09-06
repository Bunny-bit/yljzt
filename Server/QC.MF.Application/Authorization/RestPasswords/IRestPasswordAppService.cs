using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.Authorization.RestPasswords.Dto;
using QC.MF.Users.Dto;
using QC.MF.Authorization.Dto;

namespace QC.MF.Authorization.RestPasswords
{
    /// <summary>
    /// 重置 密码（找回密码）
    /// </summary>
    public interface IRestPasswordAppService : IApplicationService
    {
        /// <summary>
        /// 发送邮箱验证码   
        /// (使用者：backknow)  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendEmailCode(SendConfirmEmailByCaptchaInput input);

        /// <summary>
        /// 注册时发送手机证码  
        /// （callback）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendPhoneNumberCode(PhoneWithCaptchaInput input);

        /// <summary>
        /// 通过手机号找回密码
        /// （callback）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ResetPasswordByPhoneNumber(ResetPasswordByPhoneNumberInput input);

        /// <summary>
        /// 通过邮箱找回密码
        /// （backknow）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ResetPasswordByEmail(ResetPasswordByEmailInput input);
    }
}
