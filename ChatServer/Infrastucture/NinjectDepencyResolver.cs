using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using ChatServer.DAL;

namespace ChatServer.Infrastucture
{
    public class NinjectDepencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDepencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IRepository>().To<EFUserRepository>();
        }
    }
}