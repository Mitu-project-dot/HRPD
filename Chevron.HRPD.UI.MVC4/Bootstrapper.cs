using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using Chevron.HRPD.Common.Interfaces;
//using Chevron.HRPD.DataAccess.EntityFramework;
using Chevron.HRPD.BusinessEntities;

namespace Chevron.HRPD.UI.MVC4
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // Samples from demo project
            //container.RegisterType<IPersistence<Customer>, CustomerPersistence>();
            //container.RegisterType<IPersistence<Address>, AddressPersistence>();
            //container.RegisterType<IAddressPersistence, AddressPersistence>();
            //container.RegisterType<ICustomerPersistence, CustomerPersistence>();
            


            return container;
        }
    }
}