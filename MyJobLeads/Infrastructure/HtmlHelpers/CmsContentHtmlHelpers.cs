using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using MyJobLeads.Infrastructure.Attributes;
using Castle.Windsor.Installer;
using MyJobLeads.DomainModel.Entities.EF;

namespace MyJobLeads.Infrastructure.HtmlHelpers
{
    public static class CmsContentHtmlHelpers
    {
        public static IHtmlString GetCmsContent(this HtmlHelper html, string contentId)
        {
            // Get the database context from Windsor
            var container = new WindsorContainer();
            container.Install(FromAssembly.Containing<MyJobLeadsBaseController>());
            var context = container.Resolve<MyJobLeadsDbContext>();

            // See if any content exists for this id key
            var content = context.CmsContent
                                 .Where(x => x.Id == contentId)
                                 .SingleOrDefault();

            if (content == null)
                return new HtmlString(string.Empty);

            // Make sure the content is anti-xss safe.
            return html.Raw(MerchantTribe.Web.HtmlSanitizer.MakeHtmlSafe(content.Html));
        }
    }
}