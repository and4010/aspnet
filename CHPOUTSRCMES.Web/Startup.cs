using System;
using Microsoft.Owin;
using Owin;
using CHPOUTSRCMES.Web;
using System.Collections.Generic;
using Hangfire;
using Hangfire.SqlServer;
using System.Diagnostics;

[assembly: OwinStartup(typeof(Startup))]
namespace CHPOUTSRCMES.Web
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
        }
    }
}
