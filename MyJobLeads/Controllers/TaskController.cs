using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Commands.Tasks;
using MyJobLeads.Models.Tasks;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Contacts;

namespace MyJobLeads.Controllers
{
    public partial class TaskController : MyJobLeadsBaseController
    {
        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual ActionResult Add(int companyId, int contactId = 0)
        {
            // Retrieve the specified company value
            var company = new CompanyByIdQuery(_unitOfWork).WithCompanyId(companyId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), companyId);

            // Form the view model
            var model = new EditTaskViewModel(company);

            // Create contact list
            model.CompanyContactList = new ContactsByCompanyIdQuery(_unitOfWork).WithCompanyId(companyId)
                                                                                .Execute()
                                                                                .Select(x => new SelectListItem
                                                                                {
                                                                                    Text = x.Name,
                                                                                    Value = x.Id.ToString(),
                                                                                    Selected = x.Id == contactId
                                                                                })
                                                                                .ToList();
            model.CompanyContactList.Insert(0, new SelectListItem { Text = "<None>", Value = "0", Selected = contactId == 0 });
            return View(MVC.Task.Views.Edit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            var task = new TaskByIdQuery(_unitOfWork).WithTaskId(id).Execute();
            if (task == null)
                throw new MJLEntityNotFoundException(typeof(Task), id);

            // Form the view model
            var model = new EditTaskViewModel(task);

            // Create contact list
            model.CompanyContactList = new ContactsByCompanyIdQuery(_unitOfWork).WithCompanyId(Convert.ToInt32(task.CompanyId))
                                                                                .Execute()
                                                                                .Select(x => new SelectListItem
                                                                                {
                                                                                    Text = x.Name,
                                                                                    Value = x.Id.ToString(),
                                                                                    Selected = x.Id == Convert.ToInt32(task.ContactId)
                                                                                })
                                                                                .ToList();
            model.CompanyContactList.Insert(0, new SelectListItem { Text = "<None>", Value = "0", Selected = Convert.ToInt32(task.ContactId) == 0 });
            return View(MVC.Task.Views.Edit, model);
        }
        
        [HttpPost]
        public virtual ActionResult Edit(EditTaskViewModel model)
        {
            Task task;

            // Determine if this is a new task or not
            if (model.Id == 0)
            {
                task = new CreateTaskCommand(_unitOfWork).WithCompanyId(model.AssociatedCompanyId)
                                                         .SetName(model.Name)
                                                         .SetTaskDate(model.TaskDate)
                                                         .WithContactId(model.AssociatedContactId)
                                                         .RequestedByUserId(CurrentUserId)
                                                         .Execute();
            }
            else
            {
                // Existing task
                task = new EditTaskCommand(_unitOfWork).WithTaskId(model.Id)
                                                       .SetName(model.Name)
                                                       .SetTaskDate(model.TaskDate)
                                                       .SetContactId(model.AssociatedContactId)
                                                       .RequestedByUserId(CurrentUserId)
                                                       .Execute();
            }

            return RedirectToAction(MVC.Task.Details(task.Id));
        }

        public virtual ActionResult Details(int id)
        {
            var task = new TaskByIdQuery(_unitOfWork).WithTaskId(id).Execute();
            return View(task);
        }
    }
}
