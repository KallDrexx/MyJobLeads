using System.Web.Mvc;

namespace MyJobLeads.Areas.MyConsole
{
    public class MyConsoleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MyConsole";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MyConsole_default",
                "MyConsole/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
