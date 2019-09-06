using Abp.Configuration;
using Abp.Domain.Services;
using QC.MF.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.SMSs
{
    public class QCSMSSenderManager : MFDomainServiceBase, ISMSSenderManager
    {
        public async Task<string> Sender(string phones, string content)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = global::System.Net.DecompressionMethods.None };

            using (var http = new HttpClient(handler))
            {
                http.BaseAddress = new Uri(SettingManager.GetSettingValue(AppSettingNames.SMS.QC.Url));
                //使用FormUrlEncodedContent做HttpContent
                var urlContent = new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "Account", SettingManager.GetSettingValue(AppSettingNames.SMS.QC.Username)},
                        { "Password", SettingManager.GetSettingValue(AppSettingNames.SMS.QC.Password) },
                        { "Mobiles", phones },
                        { "Content", content}
                    });

                //await异步等待回应

                var response = await http.PostAsync(SettingManager.GetSettingValue(AppSettingNames.SMS.QC.Url), urlContent);
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();

                return result;
            }
        }

        public async Task SendVerificationCode(string phoneNumber, string code)
        {
            await Sender(phoneNumber, $"【{SettingManager.GetSettingValue(AppSettingNames.SMS.FreeSignName)}】验证码：{code}");
        }
    }
}
