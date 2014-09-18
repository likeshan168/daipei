using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using Ninject;
using System.Web.Http.Dependencies;
using Ninject.Syntax;
using Ninject.Activation;
using Ninject.Parameters;
namespace cosen.Models
{

    public class NinjectScope : IDependencyScope
    {
        private IResolutionRoot root;
        public NinjectScope(IResolutionRoot kernel)
        {
            this.root = kernel;
        }
        public object GetService(Type serviceType)
        {
            IRequest request = root.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return root.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = root.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return root.Resolve(request).ToList();
        }

        public void Dispose()
        {
            IDisposable disposable = (IDisposable)root;
            if (disposable != null) disposable.Dispose();
            root = null;
        }
    }
    //这个是专门用于webapi的
    public class NinjectAPIDependencyResolver : NinjectScope, IDependencyResolver
    {
        private IKernel kernel;
        public NinjectAPIDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
            kernel.Settings.InjectNonPublic = true;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectScope(kernel.BeginBlock());
        }

    }

    //这个是用于普通的mvc
    public class NinjectMVCDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;
        public NinjectMVCDependencyResolver()
        {
            this.kernel = new StandardKernel();
            this.kernel.Settings.InjectNonPublic = true;
            this.AddBindings();
        }

        private void AddBindings()
        {
            this.kernel.Bind<ILogicModel>().To<LogicModel>();
        }

        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }
    }
}