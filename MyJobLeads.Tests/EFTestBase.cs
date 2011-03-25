using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Data;
using System.Transactions;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Entity;

namespace MyJobLeads.Tests
{
    [TestClass]
    public class EFTestBase
    {
        protected IUnitOfWork _unitOfWork;
        protected TransactionScope _transaction;

        [TestInitialize]
        public void Setup()
        {
            Database.SetInitializer<MyJobLeadsDbContext>(new DropCreateDatabaseAlways<MyJobLeadsDbContext>());
            _unitOfWork = new EFUnitOfWork();
            _unitOfWork.UnitTestEntities.Fetch().Count();

            _transaction = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Dispose the transaction
            _transaction.Dispose();
        }

        
    }
}
