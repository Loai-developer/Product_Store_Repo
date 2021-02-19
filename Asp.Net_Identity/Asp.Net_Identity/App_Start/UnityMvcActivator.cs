using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Asp.Net_Identity.App_Start.UnityMvcActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Asp.Net_Identity.App_Start.UnityMvcActivator), "Shutdown")]


namespace Asp.Net_Identity.App_Start
{
    public class UnityMvcActivator
    {
        public static void Start()
        {
            var resolver = new UnityDependencyResolver(UnityConfig.Container);
            DependencyResolver.SetResolver(resolver) ;
            //GlobalConfiguration.Configuration.DependencyResolver = resolver;

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}