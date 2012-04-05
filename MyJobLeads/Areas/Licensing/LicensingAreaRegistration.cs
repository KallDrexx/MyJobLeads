using System.Web.Mvc;

namespace MyJobLeads.Areas.Licensing
{
    public class LicensingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Licensing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Licensing_default",
                "Licensing/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
