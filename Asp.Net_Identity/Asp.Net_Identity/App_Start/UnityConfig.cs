using Asp.Net_Identity.Controllers;
using Repository.GeneralRepository;
using ServiceLayer.ProductService;
using System;
using System.Linq;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using Unity.RegistrationByConvention;

namespace Asp.Net_Identity
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> container =
         new Lazy<IUnityContainer>(() =>
         {
             var container = new UnityContainer();
             RegisterComponents(container);
             return container;
         });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container = container.Value;

        public static void RegisterComponents(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            //container.RegisterType<IProductServiceLayer, ProductServiceLayer>();

            container.RegisterTypes(
            AllClasses.FromLoadedAssemblies()
            .Where(t => t.Name.EndsWith("ServiceLayer")),
            WithMappings.FromAllInterfaces, overwriteExistingMappings: false);
            container.RegisterType<AccountController>(new InjectionConstructor());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
    }
}