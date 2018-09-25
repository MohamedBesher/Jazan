using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Saned.Jazan.ControlPanel
{
    public class UnityDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            this._container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }

      
    }

}