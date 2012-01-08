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

namespace MyJobLeads.Areas.ContactSearch.Controllers
{
    [RequiresActiveJobSearch]
    [MJLAuthorize]
    public partial class SyncController : MyJobLeadsBaseController
    {
        protected ContactByIdQuery _contactByIdQuery;
        protected EditContactCommand _editContactCmd;

        public SyncController(MyJobLeadsDbContext context, ContactByIdQuery contactByIdQuery, EditContactCommand editContactCmd)
        {
            _context = context;
            _contactByIdQuery = contactByIdQuery;
            _editContactCmd = editContactCmd;
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