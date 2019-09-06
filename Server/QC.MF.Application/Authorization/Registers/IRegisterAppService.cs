using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.Authorization.Registers.Dto;
using QC.MF.Users.Dto;
using QC.MF.Authorization.Dto;

namespace QC.MF.Authorization.Registers
{
    /// <summary>
    /// 账号服务接口
    /// </summary>
    public interface IRegisterAppService : IApplicationService
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegisterOutput> Register(RegisterInput input);
        /// <summary>
        /// 通过邮箱注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegisterOutput> RegisterByEmail(RegisterByEmailInput input);
        /// <summary>
        /// 通过手机号注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegisterOutput> RegisterByPhoneNumber(RegisterByPhoneNumberInput input);

        /// <summary>
        /// 通过手机号注册（只有手机号）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegisterOutput> RegisterOnlyPhoneNumber(RegisterOnlyPhoneNumberInput input);

        /// <summary>
        /// 发送邮箱验证码   
        /// (使用者：registerByEmail)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendEmailCode(SendConfirmEmailByCaptchaInput input);

        /// <summary>
        /// 注册时发送手机证码  
        /// （register）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendPhoneNumberCode(PhoneWithCaptchaInput input);
    }
}
