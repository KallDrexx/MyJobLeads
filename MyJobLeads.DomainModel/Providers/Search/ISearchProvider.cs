using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ViewModels;

namespace MyJobLeads.DomainModel.Providers.Search
{
    public interface ISearchProvider
    {
        void Add(Company company);
        void Add(Contact contact);
        void Add(Task task);

        void Remove(Company company);
        void Remove(Contact contact);
        void Remove(Task task);

        SearchProviderResult Search(string searchString);
    }
}
