using Abp.Configuration;
using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using QC.MF.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace QC.MF.JPush
{
    public class JPushService : MFDomainServiceBase, IJPushService
    {
        public string app_key=> SettingManager.GetSettingValue(AppSettingNames.Push.JPush.JPushAppKey);
        private string master_secret=> SettingManager.GetSettingValue(AppSettingNames.Push.JPush.JPushMasterSecret);
        

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="alert">弹窗</param>
        /// <param name="content">消息内容</param>
        /// <param name="extras">参数</param>
        /// <param name="targets">目标(istag=true:角色表FID  istag=false:用户表FNumber)</param>
        /// <param name="type">推送目标</param>
        public void Push(string alert, PushParam content,
            string[] targets, PushType type = PushType.Alias)
        {
            try
            {
                JPushClient client = new JPushClient(app_key, master_secret);
                PushPayload payload = new PushPayload();
                payload.platform = Platform.android_ios();
                payload.options.apns_production = true;
                if (type == PushType.Tag)
                {
                    payload.audience = Audience.s_tag(targets);
                }
                else if (type == PushType.Alias)
                {
                    payload.audience = Audience.s_alias(targets);
                }
                else if (type == PushType.All)
                {
                    payload.audience = Audience.all();
                }
                var message = Message.content(content.Text);
                if (!string.IsNullOrEmpty(alert))
                {
                    var notification = new Notification().setAlert(alert);
                    notification.IosNotification = new IosNotification();
                    notification.IosNotification.incrBadge(1);
                    notification.IosNotification.setContentAvailable(true);
                    notification.IosNotification.setSound("happy");
                    notification.AndroidNotification = new AndroidNotification();
                    var extras = content.ToDictionary();
                    if (extras != null)
                    {
                        foreach (var i in extras)
                        {
                            message.AddExtras(i.Key, i.Value);
                            notification.AndroidNotification.AddExtra(i.Key, i.Value);
                            notification.IosNotification.AddExtra(i.Key, i.Value);
                        }
                    }
                    payload.notification = notification.Check();
                }
                payload.message = message.Check();
                var result = client.SendPush(payload);
            }
            catch (APIRequestException e)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Error response from JPush server. Should review and fix it. ");
                builder.Append("HTTP Status: " + e.Status);
                builder.Append("Error Code: " + e.ErrorCode);
                builder.Append("Error Message: " + e.ErrorCode);
                //throw new Exception(builder.ToString());
            }
            catch (APIConnectionException e)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Error response from JPush server. Should review and fix it. ");
                builder.Append("Message: " + e.Message);
                builder.Append("StackTrace: " + e.StackTrace);
                //throw new Exception(builder.ToString());
            }
            catch (Exception e)
            {
            }
        }
    }
}
