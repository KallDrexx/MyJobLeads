using System.Web.Mvc;

namespace MyJobLeads.Areas.PositionSearch
{
    public class PositionSearchAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PositionSearch";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PositionSearch_default",
                "PositionSearch/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
