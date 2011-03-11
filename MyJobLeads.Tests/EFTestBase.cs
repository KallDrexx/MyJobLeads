using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Data;
using System.Transactions;
using System.Data.Entity.Database;
using MyJobLeads.DomainModel.Entities.EF;

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
            DbDatabase.SetInitializer<MyJobLeadsDbContext>(new DropCreateDatabaseAlways<MyJobLeadsDbContext>());
            _unitOfWork = new EFUnitOfWork();
            _unitOfWork.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Dispose the transaction
            _unitOfWork.EndTransaction(false);
        }
    }
}
