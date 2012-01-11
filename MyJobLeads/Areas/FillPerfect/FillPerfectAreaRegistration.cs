using System.Web.Mvc;

namespace MyJobLeads.Areas.FillPerfect
{
    public class FillPerfectAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FillPerfect";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FillPerfect_default",
                "FillPerfect/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
