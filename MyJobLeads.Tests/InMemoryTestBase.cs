using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Data;

namespace MyJobLeads.Tests
{
    [TestClass]
    public class InMemoryTestBase : TestingBase
    {
        protected IUnitOfWork _unitOfWork;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new InMemoryUnitOfWork();
        }
    }
}
