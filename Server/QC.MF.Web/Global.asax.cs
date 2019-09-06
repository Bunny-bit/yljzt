using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using System.Linq;
using Abp.Localization;

namespace QC.MF.Web
{
    public class MvcApplication : AbpWebApplication<MFWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.config"))
            );
            
            base.Application_Start(sender, e);
        }
        protected override void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Context.Request.FilePath == "/") Context.RewritePath("index.html");
            // 篡改前端的语言， 强制改为中文。   如果要启用多语言管理，此处应关闭。
            //var langCookie = Request.Cookies["Abp.Localization.CultureName"];
            //if (langCookie != null)
            //{
            //    langCookie.Value = "zh-CN";
            //}
            base.Application_BeginRequest(sender, e);
        }
    }
}
