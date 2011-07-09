using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;

namespace MyJobLeads.DomainModel.Commands.JobSearches
{
    /// <summary>
    /// Command that edits the properties of a job search
    /// </summary>
    public class EditJobSearchCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected int _jobSearchId, _userId;
        protected string _newName, _newDesc;
        protected IList<string> _hiddenStatuses;
        protected bool _resetHiddenStatusList;

        public EditJobSearchCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hiddenStatuses = new List<string>();
        }

        /// <summary>
        /// Specifies the id value of the job search to edit
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        public EditJobSearchCommand WithJobSearchId(int jobSearchId)
        {
            _jobSearchId = jobSearchId;
            return this;
        }

        /// <summary>
        /// Specifies the name to set the job search to
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EditJobSearchCommand SetName(string name)
        {
            _newName = name;
            return this;
        }

        /// <summary>
        /// Specifies the new description for the job search
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public EditJobSearchCommand SetDescription(string description)
        {
            _newDesc = description;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user editing the job search
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EditJobSearchCommand CalledByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies a company status to hide from the company list
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public EditJobSearchCommand HideCompanyStatus(string status)
        {
            _hiddenStatuses.Add(status);
            return this;
        }

        /// <summary>
        /// Specifies that the hidden company status list should be reset
        /// </summary>
        /// <returns></returns>
        public EditJobSearchCommand ResetHiddenCompanyStatusList()
        {
            _resetHiddenStatusList = true;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified job search is not found</exception>
        public virtual JobSearch Execute()
        {
            // Retrieve the job search
            var search = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(_jobSearchId).Execute();
            if (search == null)
                throw new MJLEntityNotFoundException(typeof(JobSearch), _jobSearchId);

            // Retrieve the calling user
            var user = new UserByIdQuery(_unitOfWork).WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Only edit the properties that were requested
            if (_newName != null)
                search.Name = _newName;

            if (_newDesc != null)
                search.Description = _newDesc;

            if (_resetHiddenStatusList)
                search.HiddenCompanyStatuses = string.Empty;

            // Append to the hidden company status list
            if (_hiddenStatuses.Count > 0)
                foreach (string status in _hiddenStatuses)
                    search.HiddenCompanyStatuses += status + ";";

            // Create the history record
            search.History.Add(new JobSearchHistory
            {
                HistoryAction = MJLConstants.HistoryUpdate,
                DateModified = DateTime.Now,
                AuthoringUser = user,
                Name = search.Name,
                Description = search.Description,
                HiddenCompanyStatuses = search.HiddenCompanyStatuses
            });

            // Commit
            _unitOfWork.Commit();
            return search;
        }
    }
}
