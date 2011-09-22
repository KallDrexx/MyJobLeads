using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Organizations;
using MyJobLeads.ViewModels.Organizations;
using MyJobLeads.DomainModel.Commands.Organizations;

namespace MyJobLeads.Areas.Organization.Controllers
{
    public partial class EditOrganizationController : MyJobLeadsBaseController
    {
        public EditOrganizationController(IServiceFactory factory)
        {
            _serviceFactory = factory;
            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
        }

        public virtual ActionResult Index(int id)
        {
            var org = _serviceFactory.GetService<OrganizationByIdQuery>()
                                     .Execute(new OrganizationByIdQueryParams { OrganizationId = id });

            var model = new EditOrganizationViewModel(org);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(EditOrganizationViewModel model)
        {
            // Make sure a domain was entered if DomainRestricted is true
            if (model.IsEmailDomainRestricted && string.IsNullOrWhiteSpace(model.RestrictedDomain))
            {
                ModelState.AddModelError("RestrictedDomain", "When restricting registration to a specific email domain, a valid domain name is required");
                return View(model);
            }

            _serviceFactory.GetService<EditOrganizationCommand>()
                           .Execute(new EditOrganizationCommandParams
                           {
                               OrganizationId = model.Id,
                               Name = model.Name,
                               IsEmailDomainRestricted = model.IsEmailDomainRestricted,
                               EmailDomainRestriction = model.RestrictedDomain
                           });

            return RedirectToAction(MVC.Organization.Dashboard.Index());
        }
    }
}
