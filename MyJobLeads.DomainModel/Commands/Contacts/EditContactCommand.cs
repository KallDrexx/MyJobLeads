using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using FluentValidation;

namespace MyJobLeads.DomainModel.Commands.Contacts
{
    /// <summary>
    /// Command class for editing a specific contact
    /// </summary>
    public class EditContactCommand
    {
        protected IServiceFactory _serviceFactory;
        protected int _contactId, _userid;
        protected int? _jigsawId;
        protected string _name, _directPhone, _mobilePhone, _ext, _email, _assistant, _referredBy, _notes, _title;

        public EditContactCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Specifies the id value of the contact to edit
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public EditContactCommand WithContactId(int contactId)
        {
            _contactId = contactId;
            return this;
        }

        /// <summary>
        /// Specifies the name for the contact
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EditContactCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the contact's direct phone number
        /// </summary>
        /// <param name="directPhone"></param>
        /// <returns></returns>
        public EditContactCommand SetDirectPhone(string directPhone)
        {
            _directPhone = directPhone;
            return this;
        }

        /// <summary>
        /// Specifies the contact's mobile phone number
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EditContactCommand SetMobilePhone(string mobilePhone)
        {
            _mobilePhone = mobilePhone;
            return this;
        }

        /// <summary>
        /// Specifies the contact's extension
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EditContactCommand SetExtension(string extension)
        {
            _ext = extension;
            return this;
        }

        /// <summary>
        /// Specifies the contact's email
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EditContactCommand SetEmail(string email)
        {
            _email = email;
            return this;
        }

        /// <summary>
        /// Specifies the name of the contact's assistant
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EditContactCommand SetAssistant(string assistant)
        {
            _assistant = assistant;
            return this;
        }

        /// <summary>
        /// Specifies who the contact was referred by
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EditContactCommand SetReferredBy(string referredBy)
        {
            _referredBy = referredBy;
            return this;
        }

        /// <summary>
        /// Specifies the notes for the contact
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EditContactCommand SetNotes(string notes)
        {
            _notes = notes;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user editing the contact
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EditContactCommand RequestedByUserId(int userId)
        {
            _userid = userId;
            return this;
        }

        /// <summary>
        /// Specifies the title for the contact
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public EditContactCommand SetTitle(string title)
        {
            _title = title;
            return this;
        }

        /// <summary>
        /// Specifies the jigsaw id for the contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditContactCommand SetJigsawId(int id)
        {
            _jigsawId = id;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the contact is not found</exception>
        public virtual Contact Execute()
        {
            var unitofWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the user editing the contact
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(_userid).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userid);

            // Retrieve the contact
            var contact = _serviceFactory.GetService<ContactByIdQuery>().WithContactId(_contactId).RequestedByUserId(_userid).Execute();
            if (contact == null)
                throw new MJLEntityNotFoundException(typeof(Contact), _contactId);

            // Change any of the contact's properties only if the new property value is not null
            if (_name != null) { contact.Name = _name; }
            if (_directPhone != null) { contact.DirectPhone = _directPhone; }
            if (_mobilePhone != null) { contact.MobilePhone = _mobilePhone; }
            if (_ext != null) { contact.Extension = _ext; }
            if (_email != null) { contact.Email = _email; }
            if (_assistant != null) { contact.Assistant = _assistant; }
            if (_referredBy != null) { contact.ReferredBy = _referredBy; }
            if (_notes != null) { contact.Notes = _notes; }
            if (_title != null) { contact.Title = _title; }
            if (_jigsawId != null) { contact.JigsawId = (int)_jigsawId; }

            // Create a history record
            contact.History.Add(new ContactHistory
            {
                Name = contact.Name,
                DirectPhone = contact.DirectPhone,
                MobilePhone = contact.MobilePhone,
                Extension = contact.Extension,
                Email = contact.Email,
                Assistant = contact.Assistant,
                ReferredBy = contact.ReferredBy,
                Notes = contact.Notes,
                Title = contact.Title,
                JigsawId = contact.JigsawId,
                HasJigsawAccess = contact.HasJigsawAccess,

                AuthoringUser = user,
                HistoryAction = MJLConstants.HistoryUpdate,
                DateModified = DateTime.Now
            });

            // Perform Validation
            var validator = _serviceFactory.GetService<IValidator<Contact>>();
            validator.ValidateAndThrow(contact);

            // Submit changes
            unitofWork.Commit();

            // Update the contact's search index
            _serviceFactory.GetService<ISearchProvider>().Index(contact);

            return contact;
        }
    }
}
