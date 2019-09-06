using System;
using System.Drawing;
using System.Web;
using Abp.Configuration;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using Abp.UI;
using Newtonsoft.Json;
using QC.MF.Configuration;
using QC.MF.Geetests;

namespace QC.MF.Captcha
{
    public class CaptchaManager : MFDomainServiceBase, ICaptchaManager
    {
        private readonly ICacheManager _cacheManager;

        public CaptchaManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        /// <inheritdoc />
        public void CheckCaptcha(string inputCaptcha)
        {
            var requestCookie = HttpContext.Current.Request.Cookies.Get("ClientToken");
            var clientToken = "";
            if (requestCookie == null)
            {
                throw new UserFriendlyException("您的操作有误，请刷新重试");
            }
            clientToken = requestCookie.Value;
            var verifcationCache = _cacheManager.GetCache("ClientToken").GetOrDefault<string, VerifcationCache>(clientToken);
            switch (verifcationCache?.VerifcationType)
            {
                case VerifcationType.Image:
                    if (string.IsNullOrEmpty(inputCaptcha))
                    {
                        throw new UserFriendlyException("请输入验证码");
                    }
                    if (string.IsNullOrEmpty(verifcationCache?.Code))
                    {
                        throw new UserFriendlyException("验证码已过期，请刷新重试");
                    }
                    if (inputCaptcha.ToLower().Trim() != verifcationCache?.Code?.ToLower())
                    {
                        throw new UserFriendlyException("验证码输入错误");
                    }
                    _cacheManager.GetCache("ClientToken").Set(clientToken, "");
                    break;
                case VerifcationType.Drag:
                case VerifcationType.Geetest:
                    if (string.IsNullOrEmpty(inputCaptcha)
                        || string.IsNullOrEmpty(verifcationCache?.Code)
                        || inputCaptcha.ToLower().Trim() != verifcationCache?.Code?.ToLower())
                    {
                        throw new UserFriendlyException("验证码验证失败");
                    }
                    break;
                case VerifcationType.GeetestNow:
                    GeetestLib geetest = GetGeetestLib();
                    GeetestCheck input = JsonConvert.DeserializeObject<GeetestCheck>(inputCaptcha);
                    int result = geetest.enhencedValidateRequest(input.Challenge, input.Validate, input.Seccode);
                    if (result != 1)
                    {
                        throw new UserFriendlyException("验证失败");
                    }
                    break;
                default:
                    break;
            }
        }
        private GeetestLib GetGeetestLib()
        {
            return new GeetestLib(SettingManager.GetSettingValue(AppSettingNames.Captcha.Geetest.PublicKey), SettingManager.GetSettingValue(AppSettingNames.Captcha.Geetest.PrivateKey));
        }


        /// <inheritdoc />
        public Image GetCaptchaImage(int width = 80, int height = 40)
        {
            var buider = new ValidateImageBuilderOne { Height = height, Width = width };
            var content = CodeMaker.MakeCode();

            var requestCookie = HttpContext.Current.Request.Cookies.Get("ClientToken");
            var clientToken = "";
            if (requestCookie == null)
            {
                clientToken = Guid.NewGuid().ToString("N");
                var cookie = new HttpCookie("ClientToken", clientToken);
                HttpContext.Current.Response.SetCookie(cookie);
            }
            else
            {
                clientToken = requestCookie.Value;
            }
            VerifcationCache verifcationCache = new VerifcationCache()
            {
                Code = content
            };
            _cacheManager.GetCache("ClientToken").Set(clientToken, verifcationCache);

            return buider.CreateImage(content);
        }
    }
}
