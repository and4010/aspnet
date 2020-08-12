using System;
using Microsoft.Owin;
using Owin;
using CHPOUTSRCMES.Web;
using System.Collections.Generic;
using Hangfire;
using Hangfire.SqlServer;
using System.Diagnostics;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using CHPOUTSRCMES.Web.App_Start;

[assembly: OwinStartup(typeof(Startup))]
namespace CHPOUTSRCMES.Web
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            AuthConfig.Register(app);
        }
    }
}
