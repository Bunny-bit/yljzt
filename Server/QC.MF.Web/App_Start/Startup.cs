using Abp.Owin;
using Abp.Timing;
using QC.MF.Api.Controllers;
using QC.MF.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace QC.MF.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Index")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.MapSignalR();
        }
    }
}
