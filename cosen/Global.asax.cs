using cosen.Models;
using cosen3.Filters;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace cosen
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());//从新设置控制器工厂(这种方式不能用)
            //IKernel kernel = new StandardKernel();
            //webapi 和mvc 可以同时运行
            DependencyResolver.SetResolver(new NinjectMVCDependencyResolver());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //new InitializeSimpleMembershipAttribute().OnActionExecuting();
        }
    }

    //public class WebApiApplication : NinjectHttpApplication
    //{
    //    protected override IKernel CreateKernel()
    //    {
    //        var kernel = new StandardKernel();
    //        kernel.Load(Assembly.GetExecutingAssembly());
    //        //kernel.Bind<ILogicModel>().To<ILogicModel>();
    //        GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
    //        return kernel;
    //    }
    //    protected override void OnApplicationStarted()
    //    {
    //        base.OnApplicationStarted();
    //        AreaRegistration.RegisterAllAreas();

    //        WebApiConfig.Register(GlobalConfiguration.Configuration);
    //        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    //        RouteConfig.RegisterRoutes(RouteTable.Routes);
    //        BundleConfig.RegisterBundles(BundleTable.Bundles);
    //    }
    //}
}