/* Auto-generated code, do not modify */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Commands.Tasks;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Commands.Companies;
using MyJobLeads.DomainModel.Commands.JobSearches;
using MyJobLeads.DomainModel.Commands.Search;
using MyJobLeads.DomainModel.Queries.Search;

namespace MyJobLeads.Infrastructure.Installers
{
    public class CommandAndQueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
			// Command registration
			container.Register(Component.For<CreateUserCommand>().ImplementedBy<CreateUserCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<CreateTaskCommand>().ImplementedBy<CreateTaskCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<EditContactCommand>().ImplementedBy<EditContactCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<CreateCompanyCommand>().ImplementedBy<CreateCompanyCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<EditJobSearchCommand>().ImplementedBy<EditJobSearchCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<CreateJobSearchForUserCommand>().ImplementedBy<CreateJobSearchForUserCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<EditUserCommand>().ImplementedBy<EditUserCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<EditTaskCommand>().ImplementedBy<EditTaskCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<EditCompanyCommand>().ImplementedBy<EditCompanyCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<ResetUserPasswordCommand>().ImplementedBy<ResetUserPasswordCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<CreateContactCommand>().ImplementedBy<CreateContactCommand>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<RefreshSearchIndexCommand>().ImplementedBy<RefreshSearchIndexCommand>().LifeStyle.PerWebRequest);			

			// Query registration
			container.Register(Component.For<UserByEmailQuery>().ImplementedBy<UserByEmailQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<JobSearchesByUserIdQuery>().ImplementedBy<JobSearchesByUserIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<ContactsByCompanyIdQuery>().ImplementedBy<ContactsByCompanyIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<TaskByIdQuery>().ImplementedBy<TaskByIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<CompanyByIdQuery>().ImplementedBy<CompanyByIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<UserByCredentialsQuery>().ImplementedBy<UserByCredentialsQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<JobSearchByIdQuery>().ImplementedBy<JobSearchByIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<ContactByIdQuery>().ImplementedBy<ContactByIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<UserByIdQuery>().ImplementedBy<UserByIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<TasksByCompanyIdQuery>().ImplementedBy<TasksByCompanyIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<CompaniesByJobSearchIdQuery>().ImplementedBy<CompaniesByJobSearchIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<TasksByContactIdQuery>().ImplementedBy<TasksByContactIdQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<OpenTasksByJobSearchQuery>().ImplementedBy<OpenTasksByJobSearchQuery>().LifeStyle.PerWebRequest);			
			container.Register(Component.For<EntitySearchQuery>().ImplementedBy<EntitySearchQuery>().LifeStyle.PerWebRequest);				
		}
	}
}