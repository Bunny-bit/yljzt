using Abp.Configuration;
using Abp.Domain.Services;
using Aliyun.MNS;
using Aliyun.MNS.Model;
using QC.MF.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.SMSs
{
    public class AliSMSSenderManager : MFDomainServiceBase, ISMSSenderManager
    {
        public async Task SendVerificationCode(string phoneNumber, string code)
        {
            /**
             * Step 1. 初始化Client
             */
            IMNS client = new Aliyun.MNS.MNSClient(
                SettingManager.GetSettingValue(AppSettingNames.SMS.Ali.AccessKeyId),
                SettingManager.GetSettingValue(AppSettingNames.SMS.Ali.SecretAccessKey),
                SettingManager.GetSettingValue(AppSettingNames.SMS.Ali.RegionEndpoint)
                );
            /**
             * Step 2. 获取主题引用
             */
            Topic topic = client.GetNativeTopic(SettingManager.GetSettingValue(AppSettingNames.SMS.Ali.TopicName));
            /**
             * Step 3. 生成SMS消息属性
             */
            MessageAttributes messageAttributes = new MessageAttributes();
            SmsAttributes smsAttributes = new SmsAttributes();
            // 3.1 设置发送短信的签名：SMSSignName
            smsAttributes.FreeSignName = SettingManager.GetSettingValue(AppSettingNames.SMS.FreeSignName);
            // 3.2 设置发送短信的模板SMSTemplateCode
            smsAttributes.TemplateCode = SettingManager.GetSettingValue(AppSettingNames.SMS.Ali.TemplateCode);
            Dictionary<string, string> param = new Dictionary<string, string>();
            // 3.3 （如果短信模板中定义了参数）设置短信模板中的参数，发送短信时，会进行替换
            param.Add("name", code);
            // 3.4 设置短信接收者手机号码
            smsAttributes.Receiver = phoneNumber;
            smsAttributes.SmsParams = param;
            messageAttributes.SmsAttributes = smsAttributes;
            PublishMessageRequest request = new PublishMessageRequest();
            request.MessageAttributes = messageAttributes;
            /**
             * Step 4. 设置SMS消息体（必须）
             *
             * 注：目前暂时不支持消息内容为空，需要指定消息内容，不为空即可。
             */
            request.MessageBody = "smsmessage";
            try
            {
                /**
                 * Step 5. 发布SMS消息
                 */
                PublishMessageResponse resp = topic.PublishMessage(request);
            }
            catch (Exception e)
            {
                Logger.Debug("发送短信失败", e);
            }
        }

        public Task<string> Sender(string phoneNumber, string content)
        {
            throw new NotImplementedException("未实现此接口");
        }
    }
}
