using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Areas.ContactSearch.Models.Sync;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw;
using MyJobLeads.DomainModel.Exceptions.Jigsaw;

namespace MyJobLeads.Areas.ContactSearch.Controllers
{
    [RequiresActiveJobSearch]
    [MJLAuthorize]
    public partial class SyncController : MyJobLeadsBaseController
    {
        protected ContactByIdQuery _contactByIdQuery;
        protected EditContactCommand _editContactCmd;
        protected IProcess<GetJigsawContactDetailsParams, ExternalContactSearchResultsViewModel.ContactResultViewModel> _getJsContactProc;

        public SyncController(MyJobLeadsDbContext context, 
                                ContactByIdQuery contactByIdQuery, 
                                EditContactCommand editContactCmd,
                                IProcess<GetJigsawContactDetailsParams, ExternalContactSearchResultsViewModel.ContactResultViewModel> getJsContactProc)
        {
            _context = context;
            _contactByIdQuery = contactByIdQuery;
            _editContactCmd = editContactCmd;
            _getJsContactProc = getJsContactProc;
        }

        public virtual ActionResult Jigsaw(int contactId, int jigsawId, string externalName, string externalTitle, DateTime lastUpdated, string externalEmail = "", string externalPhone = "")
        {
            var contact = _contactByIdQuery.RequestedByUserId(CurrentUserId).WithContactId(contactId).Execute();
            if (contact == null)
                throw new MJLEntityNotFoundException(typeof(Contact), contactId);

            var model = new JigsawSyncViewModel
            {
                ContactId = contactId,
                JigsawId = jigsawId,
                LastUpdatedOnJigsaw = lastUpdated,
                InternalName = contact.Name,
                InternalEmail = contact.Email,
                InternalPhone = contact.DirectPhone,
                InternalTitle = contact.Title,
                JigsawName = externalName,
                JigsawEmail = externalEmail,
                JigsawPhone = externalPhone,
                JigsawTitle = externalTitle
            };

            // If the contact is marked as having access to the contact on jigsaw, retrieve the email and phone from jigsaw
            if (contact.JigsawId == jigsawId && contact.HasJigsawAccess)
            {
                try
                {
                    var jsContact = _getJsContactProc.Execute(new GetJigsawContactDetailsParams
                    {
                        RequestingUserId = CurrentUserId,
                        PurchaseContact = false,
                        JigsawContactId = jigsawId
                    });

                    model.JigsawName = jsContact.FirstName + " " + jsContact.LastName;
                    model.JigsawTitle = jsContact.Headline;
                    model.JigsawEmail = jsContact.Email;
                    model.JigsawPhone = jsContact.Phone;
                }

                catch (JigsawException) { }
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Jigsaw(JigsawSyncViewModel model)
        {
            var contact = _contactByIdQuery.RequestedByUserId(CurrentUserId).WithContactId(model.ContactId).Execute();
            if (contact == null)
                throw new MJLEntityNotFoundException(typeof(Contact), model.ContactId);

            if (!ModelState.IsValid)
            {
                model.InternalEmail = contact.Email;
                model.InternalName = contact.Name;
                model.InternalPhone = contact.DirectPhone;
                model.InternalTitle = contact.Title;

                return View(model);
            }

            _editContactCmd.RequestedByUserId(CurrentUserId)
                           .WithContactId(model.ContactId);

            // Only update data selected to be imported
            if (model.ImportName)
                _editContactCmd.SetName(model.JigsawName);

            if (model.ImportTitle)
                _editContactCmd.SetTitle(model.JigsawTitle);

            if (model.ImportEmail)
                _editContactCmd.SetEmail(model.JigsawEmail);

            if (model.ImportPhone)
                _editContactCmd.SetDirectPhone(model.JigsawPhone);

            _editContactCmd.SetJigsawId(model.JigsawId);
            _editContactCmd.Execute();

            return RedirectToAction(MVC.Contact.Details(model.ContactId));
        }
    }
}