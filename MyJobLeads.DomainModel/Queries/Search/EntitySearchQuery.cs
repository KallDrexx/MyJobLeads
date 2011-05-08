using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.ViewModels;

namespace MyJobLeads.DomainModel.Queries.Search
{
    /// <summary>
    /// Queries for entities based on a specfic search phrase
    /// </summary>
    public class EntitySearchQuery
    {
        protected IUnitOfWork _unitOfWork;
        protected ISearchProvider _searchProvider;
        protected string _searchQuery;

        public EntitySearchQuery(IUnitOfWork unitOfWork, ISearchProvider searchProvider)
        {
            _unitOfWork = unitOfWork;
            _searchProvider = searchProvider;
        }

        /// <summary>
        /// Specifies the search string to search for
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public EntitySearchQuery WithSearchQuery(string searchQuery)
        {
            _searchQuery = searchQuery;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public SearchResultEntities Execute()
        {
            // Perform the search
            var results = _searchProvider.Search(_searchQuery);

            // Go through all the returned id values and retrieve the entities for them
            var companies = _unitOfWork.Companies.Fetch().Where(x => results.FoundCompanyIds.Contains(x.Id)).ToList();
            var contacts = _unitOfWork.Contacts.Fetch().Where(x => results.FoundContactIds.Contains(x.Id)).ToList();
            var tasks = _unitOfWork.Tasks.Fetch().Where(x => results.FoundTaskIds.Contains(x.Id)).ToList();

            return new SearchResultEntities
            {
                Companies = companies,
                Contacts = contacts,
                Tasks = tasks
            };
        }
    }
}
