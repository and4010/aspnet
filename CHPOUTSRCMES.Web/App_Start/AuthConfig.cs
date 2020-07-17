using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace CHPOUTSRCMES.Web.App_Start
{
    public class AuthConfig
    {
        public static void Register(IAppBuilder app)
        {
            app.CreatePerOwinContext<MesContext>(MesContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
        }
    }
}