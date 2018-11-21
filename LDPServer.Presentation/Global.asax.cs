using Autofac;
using Autofac.Integration.WebApi;
using LDPServer.Business;
using LDPServer.Common.Interfaces;
using LDPServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace LDPServer.Presentation
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ResourcesService>().AsSelf().InstancePerRequest();
            builder.RegisterType<RdfService>().AsSelf().InstancePerRequest();
            builder.RegisterType<ResourceFileRepository>().As<IResourceRepository>().InstancePerRequest();
            builder.RegisterType<AppDataFolderProvider>().As<IDataFolder>().SingleInstance();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
