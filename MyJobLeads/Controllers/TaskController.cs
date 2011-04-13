using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Commands.Tasks;

namespace MyJobLeads.Controllers
{
    public partial class TaskController : MyJobLeadsBaseController
    {
        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual ActionResult AddCompanyTask(int companyId)
        {
            ViewBag.CompanyId = companyId;
            return View(MVC.Task.Views.Edit, new Task());
        }

        public virtual ActionResult AddContactTask(int contactId)
        {
            ViewBag.ContactId = contactId;
            return View(MVC.Task.Views.Edit, new Task());
        }

        public virtual ActionResult Edit(int id)
        {
            var task = new TaskByIdQuery(_unitOfWork).WithTaskId(id).Execute();
            return View(task);
        }

        [HttpPost]
        public virtual ActionResult Edit(Task task, int companyId, int contactId)
        {
            // Determine if this is a new task or not
            if (task.Id == 0)
            {
                // Determine if this task is for a company or contact
                if (companyId != 0)
                {
                    task = new CreateTaskCommand(_unitOfWork).WithCompanyId(companyId)
                                                                       .SetName(task.Name)
                                                                       .SetTaskDate(task.TaskDate)
                                                                       .RequestedByUserId(CurrentUserId)
                                                                       .Execute();
                }
                else
                {
                    task = new CreateTaskForContactCommand(_unitOfWork).WithContactId(contactId)
                                                                       .SetName(task.Name)
                                                                       .SetTaskDate(task.TaskDate)
                                                                       .RequestedByUserId(CurrentUserId)
                                                                       .Execute();
                }
            }
            else
            {
                // Existing task
                task = new EditTaskCommand(_unitOfWork).WithTaskId(task.Id)
                                                       .SetName(task.Name)
                                                       .SetTaskDate(task.TaskDate)
                                                       .SetCompleted(task.Completed)
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
