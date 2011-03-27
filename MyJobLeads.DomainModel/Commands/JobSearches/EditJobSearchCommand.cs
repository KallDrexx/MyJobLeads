using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Commands.JobSearches
{
    /// <summary>
    /// Command that edits the properties of a job search
    /// </summary>
    public class EditJobSearchCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected int _jobSearchId;
        protected string _newName, _newDesc;

        public EditJobSearchCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified job search is not found</exception>
        public JobSearch Execute()
        {
            // Retrieve the job search
            var search = new JobSearchByIdQuery(_unitOfWork).WithJobSearchId(_jobSearchId).Execute();
            if (search == null)
                throw new MJLEntityNotFoundException(typeof(JobSearch), _jobSearchId);

            // Only edit the properties that were requested
            if (_newName != null)
                search.Name = _newName;

            if (_newDesc != null)
                search.Description = _newDesc;

            // Commit
            _unitOfWork.Commit();
            return search;
        }
    }
}
