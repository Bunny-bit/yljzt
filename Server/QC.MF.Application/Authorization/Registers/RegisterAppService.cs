using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Zero.Configuration;
using QC.MF.Authorization.Registers.Dto;
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
using System;

namespace QC.MF.Authorization.Registers
{
    /// <summary>
    /// 账号服务
    /// </summary>
    public class RegisterAppService : MFAppServiceBase, IRegisterAppService
    {
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ISMSManager _smsManager;
        private readonly LogInManager _loginManager;
        private readonly PasswordComplexityChecker _passwordComplexityChecker;
        private readonly ICaptchaManager _captchaManager;
        private readonly ICacheManager _cacheManager;
        private readonly UserManager _userManager;
        static Random Random = new Random();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRegistrationManager"></param>
        /// <param name="smsManager"></param>
        /// <param name="loginManager"></param>
        /// <param name="passwordComplexityChecker"></param>
        /// <param name="captchaManager"></param>
        public RegisterAppService(
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
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                false
            );
            await _userRegistrationManager.BindingThirdPartyAsync(input.Token, user);

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendEmailCode(SendConfirmEmailByCaptchaInput input)
        {
            if ((await UserManager.FindByEmailAsync(input.Email)) != null)
            {
                throw new UserFriendlyException("当前邮箱已被注册");
            }
            await _userRegistrationManager.SendEmailCodeAsync(input.Email, input.Captcha);
        }

        /// <summary>
        /// 通过邮箱注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterOutput> RegisterByEmail(RegisterByEmailInput input)
        {
            _userRegistrationManager.ValidateEmailCode(input.EmailAddress, input.Captcha);
            CheckPasswordFormat(input.Password);
            var user = await _userRegistrationManager.RegisterAsync(
                input.UserName,
                input.UserName,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true
            );
            await _userRegistrationManager.BindingThirdPartyAsync(input.Token, user);

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        /// <summary>
        /// 注册时发送手机证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendPhoneNumberCode(PhoneWithCaptchaInput input)
        {
            //_captchaManager.CheckCaptcha(input.Captcha);
            if ((await UserManager.FindUserByPhoneNumberAsync(input.PhoneNumber)) != null)
            {
                throw new UserFriendlyException("当前手机号已被注册");
            }
            await _smsManager.SendVerificationCode(input.PhoneNumber);
        }

        /// <summary>
        /// 通过手机号注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterOutput> RegisterByPhoneNumber(RegisterByPhoneNumberInput input)
        {
            _smsManager.ValidateVerificationCode(input.PhoneNumber, input.Captcha);
            CheckPasswordFormat(input.Password);
            var user = await _userRegistrationManager.RegisterAsync(
                input.UserName,
                input.UserName,
                input.PhoneNumber,
                input.UserName,
                input.Password);
            await _userRegistrationManager.BindingThirdPartyAsync(input.Token, user);
            return new RegisterOutput
            {
                CanLogin = true
            };
        }
        /// <summary>
        /// 通过手机号注册（只有手机号）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterOutput> RegisterOnlyPhoneNumber(RegisterOnlyPhoneNumberInput input)
        {
            _smsManager.ValidateVerificationCode(input.PhoneNumber, input.Captcha);
            CheckPasswordFormat(input.Password);
            var user = await _userRegistrationManager.RegisterAsync(
                "PU"+ GetNickname(),
                "PU",
                input.PhoneNumber,
                input.PhoneNumber,
                input.Password);
            return new RegisterOutput
            {
                CanLogin = true
            };
        }
        private async Task< string> GetNickname()
        {
            var uIdmax = await _userManager.Users.MaxAsync(x => x.Id);
            return (uIdmax + 1) + "" + Random.Next(9999);
        }

        private void CheckPasswordFormat(string password)
        {
            _passwordComplexityChecker.Check(password);
        }


    }
}
