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

namespace MyJobLeads.Infrastructure.Installers
{
    public class CommandAndQueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
			// Command registration
			container.Register(Component.For<CreateUserCommand>().ImplementedBy<CreateUserCommand>());			
			container.Register(Component.For<CreateTaskCommand>().ImplementedBy<CreateTaskCommand>());			
			container.Register(Component.For<EditContactCommand>().ImplementedBy<EditContactCommand>());			
			container.Register(Component.For<CreateCompanyCommand>().ImplementedBy<CreateCompanyCommand>());			
			container.Register(Component.For<EditJobSearchCommand>().ImplementedBy<EditJobSearchCommand>());			
			container.Register(Component.For<CreateJobSearchForUserCommand>().ImplementedBy<CreateJobSearchForUserCommand>());			
			container.Register(Component.For<EditUserCommand>().ImplementedBy<EditUserCommand>());			
			container.Register(Component.For<EditTaskCommand>().ImplementedBy<EditTaskCommand>());			
			container.Register(Component.For<EditCompanyCommand>().ImplementedBy<EditCompanyCommand>());			
			container.Register(Component.For<ResetUserPasswordCommand>().ImplementedBy<ResetUserPasswordCommand>());			
			container.Register(Component.For<CreateContactCommand>().ImplementedBy<CreateContactCommand>());			

			// Query registration
			container.Register(Component.For<UserByEmailQuery>().ImplementedBy<UserByEmailQuery>());			
			container.Register(Component.For<JobSearchesByUserIdQuery>().ImplementedBy<JobSearchesByUserIdQuery>());			
			container.Register(Component.For<ContactsByCompanyIdQuery>().ImplementedBy<ContactsByCompanyIdQuery>());			
			container.Register(Component.For<TaskByIdQuery>().ImplementedBy<TaskByIdQuery>());			
			container.Register(Component.For<CompanyByIdQuery>().ImplementedBy<CompanyByIdQuery>());			
			container.Register(Component.For<UserByCredentialsQuery>().ImplementedBy<UserByCredentialsQuery>());			
			container.Register(Component.For<JobSearchByIdQuery>().ImplementedBy<JobSearchByIdQuery>());			
			container.Register(Component.For<ContactByIdQuery>().ImplementedBy<ContactByIdQuery>());			
			container.Register(Component.For<UserByIdQuery>().ImplementedBy<UserByIdQuery>());			
			container.Register(Component.For<TasksByCompanyIdQuery>().ImplementedBy<TasksByCompanyIdQuery>());			
			container.Register(Component.For<CompaniesByJobSearchIdQuery>().ImplementedBy<CompaniesByJobSearchIdQuery>());			
			container.Register(Component.For<TasksByContactIdQuery>().ImplementedBy<TasksByContactIdQuery>());			
			container.Register(Component.For<OpenTasksByJobSearchQuery>().ImplementedBy<OpenTasksByJobSearchQuery>());			
		}
	}
}