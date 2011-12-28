using System.Web.Mvc;

namespace MyJobLeads.Areas.ContactSearch
{
    public class ContactSearchAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ContactSearch";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ContactSearch_default",
                "ContactSearch/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
