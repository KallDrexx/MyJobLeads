using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Commands.Search
{
    public class RefreshSearchIndexCommand
    {
        protected IUnitOfWork _unitOfWork;
        protected ISearchProvider _searchProvider;

        public RefreshSearchIndexCommand(IUnitOfWork unitOfWork, ISearchProvider searchProvider)
        {
            _unitOfWork = unitOfWork;
            _searchProvider = searchProvider;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute()
        {
            // Retrieve all companies from the database, index them, then release them from memory
            var companies = _unitOfWork.Companies.Fetch().ToList();
            foreach (var company in companies)
                _searchProvider.Index(company);

            companies = null;
            GC.Collect();

            // Retrieve all contacts from the database, index them, then release them from memory
            var contacts = _unitOfWork.Contacts.Fetch().ToList();
            foreach (var contact in contacts)
                _searchProvider.Index(contact);

            contacts = null;
            GC.Collect();

            // Retrieve all tasks from the database, index them, then release them from memory
            var tasks = _unitOfWork.Tasks.Fetch().ToList();
            foreach (var task in tasks)
                _searchProvider.Index(task);

            tasks = null;
            GC.Collect();
        }
    }
}
