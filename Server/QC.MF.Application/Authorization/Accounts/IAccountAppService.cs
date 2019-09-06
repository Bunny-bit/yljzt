using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.Authorization.Accounts.Dto;
using QC.MF.Users.Dto;
using QC.MF.Authorization.Dto;

namespace QC.MF.Authorization.Accounts
{
    /// <summary>
    /// 账号服务接口
    /// </summary>
    public interface IAccountAppService : IApplicationService
    {
        /// <summary>
        /// 租户是否可用
        /// （无调用）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);
        /// <summary>
        /// 绑定账号
        /// （无调用）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BindingThirdParty(BindingThirdPartyInput input);

        /// <summary>
        /// 登录时发送手机证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendPhoneNumberCode(PhoneWithCaptchaInput input);
    }
}
