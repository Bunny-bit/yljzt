using Abp.Configuration;
using Abp.Runtime.Caching;
using Abp.Web.Models;
using Newtonsoft.Json;
using QC.MF.Captcha;
using QC.MF.Configuration;
using QC.MF.DragVerifications.Dto;
using QC.MF.Geetests.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QC.MF.Geetests
{
    public class GeetestAppService : MFAppServiceBase, IGeetestAppService
    {
        private readonly ICacheManager _cacheManager;
        public GeetestAppService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        private void SetCache(VerifcationCache verifcationCache)
        {
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
            if (verifcationCache != null)
            {
                _cacheManager.GetCache("ClientToken").Set(clientToken, verifcationCache);
            }
            else
            {
                _cacheManager.GetCache("ClientToken").Remove(clientToken);
            }
        }

        private VerifcationCache GetCache()
        {
            var requestCookie = HttpContext.Current.Request.Cookies.Get("ClientToken");
            if (requestCookie == null)
            {
                throw new Abp.UI.UserFriendlyException("错误的请求");
            }
            var clientToken = requestCookie.Value;
            var verifcationCache = _cacheManager.GetCache("ClientToken").GetOrDefault<string, VerifcationCache>(clientToken);
            if (verifcationCache?.VerifcationType != VerifcationType.Geetest)
            {
                throw new Abp.UI.UserFriendlyException("错误的请求");
            }
            return verifcationCache;
        }
        public string GetCaptcha()
        {
            var verifcationCache = new VerifcationCache()
            {
                VerifcationType = VerifcationType.GeetestNow
            };
            SetCache(verifcationCache);

            GeetestLib geetest = GetGeetestLib();
            Byte gtServerStatus = geetest.preProcess();
            return geetest.getResponseStr();
        }


        public CheckCodeOutput Check(GeetestCheckInput input)
        {
            GeetestLib geetest = GetGeetestLib();
            int result = geetest.enhencedValidateRequest(input.Challenge, input.Validate, input.Seccode);
            if (result == 1)
            {
                var verifcationCache = new VerifcationCache()
                {
                    VerifcationType = VerifcationType.Geetest,
                    Code = Guid.NewGuid().ToString()
                };
                SetCache(verifcationCache);
                return new CheckCodeOutput()
                {
                    Success = true,
                    Token = verifcationCache.Code
                };
            }
            else
            {
                return new CheckCodeOutput() { Success = false };
            }
        }

        public GeetestCheckOutput APPGetCaptcha()
        {
            return JsonConvert.DeserializeObject<GeetestCheckOutput>(GetCaptcha());
        }

        public string APPCheck(GeetestAppCheckInput input)
        {
            var result = Check(new GeetestCheckInput()
            {
                Challenge = input.geetest_challenge,
                Seccode = input.geetest_seccode,
                Validate = input.geetest_validate,
            });
            if (result.Success)
            {
                return "success";
            }
            else
            {
                throw new Abp.UI.UserFriendlyException("invalid");
            }
        }
        private GeetestLib GetGeetestLib()
        {
            return new GeetestLib(SettingManager.GetSettingValue(AppSettingNames.Captcha.Geetest.PublicKey), SettingManager.GetSettingValue(AppSettingNames.Captcha.Geetest.PrivateKey));
        }
    }
}
