using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Positions;

namespace MyJobLeads.Tests.Queries
{
    [TestClass]
    public class PositionByIdQueryTests : EFTestBase
    {
        [TestMethod]
        public void Can_Get_Position_By_Id()
        {
            // Setup 
            Position pos1 = new Position(), pos2 = new Position(), pos3 = new Position();
            _unitOfWork.Positions.Add(pos1);
            _unitOfWork.Positions.Add(pos2);
            _unitOfWork.Positions.Add(pos3);
            _unitOfWork.Commit();

            // Act
            Position result = new PositionByIdQuery(_serviceFactory.Object).WithPositionId(pos2.Id).Execute();

            // Verify
            Assert.IsNotNull(result, "Query returned a null position");
            Assert.AreEqual(pos2, result, "Query returned an incorrect position");
        }

        [TestMethod]
        public void Query_Returns_Null_When_Id_Not_Found()
        {
            // Setup
            Position pos = new Position();
            _unitOfWork.Positions.Add(pos);
            _unitOfWork.Commit();

            // Act
            Position result = new PositionByIdQuery(_serviceFactory.Object).WithPositionId(pos.Id + 1).Execute();

            // Verify
            Assert.IsNull(result, "Query did not return a null position");
        }
    }
}
