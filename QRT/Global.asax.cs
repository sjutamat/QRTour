using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using QRT.Domain;
using QRT.DB;

namespace QRT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var db = Assembly.GetAssembly(typeof(DAL.Implement.mas_routeRepository));
            builder.RegisterAssemblyTypes(db).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().PropertiesAutowired().InstancePerHttpRequest();


            var service = Assembly.GetAssembly(typeof(Service.Implement.mas_routeService));
            builder.RegisterAssemblyTypes(service).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().PropertiesAutowired().InstancePerHttpRequest();

            builder.RegisterType<QRCodeTourEntities>().InstancePerHttpRequest();

            builder.RegisterFilterProvider();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
