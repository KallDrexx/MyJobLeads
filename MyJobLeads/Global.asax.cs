using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Entity;
using System.Configuration;
using MyJobLeads.Infrastructure.ModelBinders;

namespace MyJobLeads
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            const string UpdateDbAppSettingName = "UpdateDatabaseOnModelChange";

            ModelBinders.Binders.DefaultBinder = new EmptyStringModelBaseBinder();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            if (ConfigurationManager.AppSettings[UpdateDbAppSettingName] == "true")
                Database.SetInitializer<MyJobLeadsDbContext>(new MyJobLeadsDbInitializer());
            else
                Database.SetInitializer<MyJobLeadsDbContext>(null);
        }
    }
}