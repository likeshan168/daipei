using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.App_Code
{
    public class ModuleDemo:IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //context.Response.Write("<h1 style='color:#00f'>来自HttpModule的处理，请求结束</h1><hr>");
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //context.Response.Write("<h1 style='color:#00f'>来自HttpModule的处理，请求达到</h1><hr>");
        }
    }
}