using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.Authorization.Actives.Dto;
using QC.MF.Users.Dto;
using QC.MF.Authorization.Dto;

namespace QC.MF.Authorization.Actives
{
    /// <summary>
    /// 账号服务接口
    /// </summary>
    public interface IActiveAppService : IApplicationService
    {
        /// <summary>
        /// 发送邮箱验证码
        /// （sendemail）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendConfirmEmailCode(SendConfirmEmailByCaptchaInput input);
        /// <summary>
        /// 发送手机激活验证码
        /// （sendemail）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendConfirmPhoneNumberByCode(VerificationCodeInput input);

        /// <summary>
        /// 验证邮箱验
        /// （sendemail）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ConfirmEmailByCode(ConfirmEmailCodeInput input);
        /// <summary>
        /// 验证手机号
        /// （sendemail）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ConfirmPhoneNumberByCode(VerificationCodeInput input);
    }
}
