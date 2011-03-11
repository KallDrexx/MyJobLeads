using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Tests.Data
{
    [TestClass]
    public class EFUnitOfWorkTests : EFTestBase
    {
        [TestMethod]
        public void Can_Add_New_Entities()
        {
            // setup
            UnitTestEntity entity = new UnitTestEntity();

            // Act
            _unitOfWork.UnitTestEntities.Add(entity);

            // Verify
            _unitOfWork.Commit();
            Assert.AreEqual(1, _unitOfWork.UnitTestEntities.Fetch().Where(x => x.Id == entity.Id).Count()
                            , "Repository has an incorrect number of entities");
        }

        [TestMethod]
        public void Can_Remove_Entities()
        {
            // Setup
            UnitTestEntity entity = new UnitTestEntity();
            _unitOfWork.UnitTestEntities.Add(entity);
            _unitOfWork.Commit();

            // Act
            _unitOfWork.UnitTestEntities.Remove(entity);
            _unitOfWork.Commit();

            // Verify
            Assert.AreEqual(0, _unitOfWork.UnitTestEntities.Fetch().Where(x => x.Id == entity.Id).Count()
                            , "Repository has an incorrect number of entities");
        }

        [TestMethod]
        public void Can_Fetch_Entities()
        {
            // Setup
            UnitTestEntity entity = new UnitTestEntity();
            _unitOfWork.UnitTestEntities.Add(entity);
            _unitOfWork.Commit();

            // Act
            UnitTestEntity result = _unitOfWork.UnitTestEntities.Fetch().Where(x => x.Id == entity.Id).Single();

            // Verify
            Assert.AreEqual(entity.Id, result.Id, "Returned entity had an incorrect Id value");
        }
    }
}
