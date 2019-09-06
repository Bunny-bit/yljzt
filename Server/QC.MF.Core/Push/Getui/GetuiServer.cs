using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.gexin.rp.sdk.dto;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using com.igetui.api.openservice.payload;
using System.Net;
using Abp.Configuration;
using QC.MF.Configuration;

/**
 * 
 * 说明：
 *      此工程是一个测试工程，所用的相关.dll文件，都已经存在protobuffer文件里，需要加载到References里。
 *      工程中还有用到一个System.Web.Extensions文件，这个文件是用到Framework里V4.0版本的，
 *      一般路径如下：C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0，
 *      或如下路径：C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.5没有的也可以在protobuffer
 *      文件夹里找到。如再有问题，请直接联系技术客服，谢谢！
 *      GetuiServerApiSDK：此.dll文件为个推C#版本的SDK文件
 *      Google.ProtocolBuffers：此.dll文件为Google的数据交换格式文件
 *  注：
 *      新增一个连接超时时间设置，通过在环境变量--用户变量中增加名为：GETUI_TIMEOUT 的变量（修改环境变量，
       电脑重启后才能生效），值则是超时时间，如不设定，则默认为20秒。
 **/

namespace QC.MF.JPush
{
    public class GetuiServer : MFDomainServiceBase, IGetuiServer
    {
        //参数设置 <-----参数需要重新设置----->
        //http的域名
        private static String HOST = "http://sdk.open.api.igexin.com/apiex.htm";

        //https的域名
        //private static String HOST = "https://api.getui.com/apiex.htm";

        private  String AppID => SettingManager.GetSettingValue(AppSettingNames.Push.Getui.AppID);
        private  String AppKey => SettingManager.GetSettingValue(AppSettingNames.Push.Getui.AppKey);
        private  String MasterSecret => SettingManager.GetSettingValue(AppSettingNames.Push.Getui.MasterSecret);
        
        
        public GetuiServer()
        {
        }

        
        //pushMessageToApp接口测试代码
        public void Push(string msg,params string[] tagList)
        {
            IGtPush push = new IGtPush(HOST, AppKey, MasterSecret);
            // 定义"AppMessage"类型消息对象，设置消息内容模板、发送的目标App列表、是否支持离线发送、以及离线消息有效期(单位毫秒)
            AppMessage appMessage = new AppMessage();

            TransmissionTemplate template = TransmissionTemplateDemo(msg);

            appMessage.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            appMessage.OfflineExpireTime = 1000 * 3600 * 12;     // 离线有效时间，单位为毫秒，可选
            appMessage.Data = template;
            //判断是否客户端是否wifi环境下推送，2:4G/3G/2G,1为在WIFI环境下，0为无限制环境
            //message.PushNetWorkType = 0; 
            //message.Speed = 1000;

            List<String> appIdList = new List<string>();
            appIdList.Add(AppID);

            List<String> phoneTypeList = new List<string>();   //通知接收者的手机操作系统类型
            //phoneTypeList.Add("ANDROID");
            //phoneTypeList.Add("IOS");

            List<String> provinceList = new List<string>();    //通知接收者所在省份
            //provinceList.Add("浙江");
            //provinceList.Add("上海");
            //provinceList.Add("北京");
            
            //tagList.Add("中文");

            appMessage.AppIdList = appIdList;
            appMessage.PhoneTypeList = phoneTypeList;
            appMessage.ProvinceList = provinceList;
            appMessage.TagList = tagList.ToList();


            String pushResult = push.pushMessageToApp(appMessage);
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服务端返回结果：" + pushResult);
        }

        //透传模板动作内容
        private  TransmissionTemplate TransmissionTemplateDemo(string message)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = AppID;
            template.AppKey = AppKey;
            //应用启动类型，1：强制应用启动 2：等待应用启动
            template.TransmissionType = "1";
            //透传内容  
            template.TransmissionContent = message;
            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
            //String begin = "2015-03-06 14:36:10";
            //String end = "2015-03-06 14:46:20";
            //template.setDuration(begin, end);

            return template;
        }
    }
}
