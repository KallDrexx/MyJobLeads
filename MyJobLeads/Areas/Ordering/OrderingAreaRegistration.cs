using System.Web.Mvc;

namespace MyJobLeads.Areas.Ordering
{
    public class OrderingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ordering";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ordering_default",
                "Ordering/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
