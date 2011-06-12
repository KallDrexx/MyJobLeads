using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Data;
using System.Transactions;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Entity;
using Moq;
using MyJobLeads.DomainModel.Providers;

namespace MyJobLeads.Tests
{
    [TestClass]
    public class EFTestBase
    {
        protected IUnitOfWork _unitOfWork;
        protected Mock<IServiceFactory> _serviceFactory;
        protected TransactionScope _transaction;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the database connection
            Database.SetInitializer<MyJobLeadsDbContext>(new DropCreateDatabaseAlways<MyJobLeadsDbContext>());
            _unitOfWork = new EFUnitOfWork();
            _unitOfWork.UnitTestEntities.Fetch().Count(); // Required to make sure database is created outside of transaction scope

            _transaction = new TransactionScope();

            // Create service factory mock
            _serviceFactory = new Mock<IServiceFactory>();
            _serviceFactory.Setup(x => x.GetService<IUnitOfWork>()).Returns(_unitOfWork);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Dispose the transaction
            _transaction.Dispose();
        }   
    }
}
