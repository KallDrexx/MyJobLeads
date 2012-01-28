using System.Web.Mvc;

namespace MyJobLeads.Areas.CompanySearch
{
    public class CompanySearchAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CompanySearch";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CompanySearch_default",
                "CompanySearch/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
