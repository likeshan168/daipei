using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
namespace cosen.Models
{
    /// <summary>
    /// 自定义控制器工厂（实现ninject 依赖注入）
    /// </summary>
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        IKernel kernel;
        public NinjectControllerFactory()
        {
            this.kernel = new StandardKernel();
            this.kernel.Settings.InjectNonPublic = true;//让私有属性也能实现注入

        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            this.AddBindings();
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
            //return base.GetControllerInstance(requestContext, controllerType);
        }
        private void AddBindings()
        {
            kernel.Bind<ILogicModel>().To<LogicModel>();
        }
    }
}