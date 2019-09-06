using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Zero.Configuration;
using QC.MF.Authorization.RestPasswords.Dto;
using QC.MF.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using Newtonsoft.Json;
using QC.MF.Authorization.ThirdParty;
using QC.MF.Configuration;
using QC.MF.Security;
using QC.MF.SMSs;
using QC.MF.Users.Dto;
using QC.MF.Captcha;
using Abp.Runtime.Caching;
using Microsoft.AspNet.Identity;
using QC.MF.Authorization.Dto;

namespace QC.MF.Authorization.RestPasswords
{
    /// <summary>
    /// 账号服务
    /// </summary>
    public class RestPasswordAppService : MFAppServiceBase, IRestPasswordAppService
    {
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ISMSManager _smsManager;
        private readonly LogInManager _loginManager;
        private readonly PasswordComplexityChecker _passwordComplexityChecker;
        private readonly ICaptchaManager _captchaManager;
        private readonly ICacheManager _cacheManager;
        private readonly UserManager _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRegistrationManager"></param>
        /// <param name="smsManager"></param>
        /// <param name="loginManager"></param>
        /// <param name="passwordComplexityChecker"></param>
        /// <param name="captchaManager"></param>
        public RestPasswordAppService(
            UserRegistrationManager userRegistrationManager,
            ISMSManager smsManager,
            LogInManager loginManager,
            PasswordComplexityChecker passwordComplexityChecker,
            ICacheManager cacheManager,
            UserManager userManager,
            ICaptchaManager captchaManager)
        {
            _userRegistrationManager = userRegistrationManager;
            _smsManager = smsManager;
            _loginManager = loginManager;
            _passwordComplexityChecker = passwordComplexityChecker;
            _cacheManager = cacheManager;
            _userManager = userManager;
            _captchaManager = captchaManager;
        }

        

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendEmailCode(SendConfirmEmailByCaptchaInput input)
        {
            if ((await UserManager.FindByEmailAsync(input.Email)) == null)
            {
                throw new UserFriendlyException("邮箱无效");
            }
            await _userRegistrationManager.SendEmailCodeAsync(input.Email, input.Captcha);
        }        

        /// <summary>
        /// 注册时发送手机证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendPhoneNumberCode(PhoneWithCaptchaInput input)
        {
            _captchaManager.CheckCaptcha(input.Captcha);
            if ((await UserManager.FindUserByPhoneNumberAsync(input.PhoneNumber)) == null)
            {
                throw new UserFriendlyException("手机号无效");
            }
            await _smsManager.SendVerificationCode(input.PhoneNumber);
        }
        
        

        /// <summary>
        /// 通过手机重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ResetPasswordByPhoneNumber(ResetPasswordByPhoneNumberInput input)
        {
            _smsManager.ValidateVerificationCode(input.PhoneNumber, input.Code);
            _passwordComplexityChecker.Check( input.Password);
            await _userRegistrationManager.ResetPasswordByPhoneNumberAsync(input.PhoneNumber, input.Password);
        }
        

        /// <summary>
        /// 通过邮箱重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ResetPasswordByEmail(ResetPasswordByEmailInput input)
        {
            await VerificationResetPasswordByEmail(input);
            _passwordComplexityChecker.Check(input.Password);

            var user = await _userManager.Users.FirstOrDefaultAsync(n => n.EmailAddress == input.Email);
            if (user == null)
            {
                throw new UserFriendlyException("找不到此邮箱关联的用户");
            }
            user.Password = new PasswordHasher().HashPassword(input.Password);
            await CurrentUnitOfWork.SaveChangesAsync();

            await _cacheManager.GetCache("EmailCode").RemoveAsync(input.Email);
        }
        private async Task VerificationResetPasswordByEmail(VerificationResetPasswordByEmailInput input)
        {
            if ((await _cacheManager.GetCache("EmailCode").GetOrDefaultAsync(input.Email)).ToString() != input.VerificationCode)
            {
                throw new UserFriendlyException("验证码输入错误");
            }
        }

        

    }
}
