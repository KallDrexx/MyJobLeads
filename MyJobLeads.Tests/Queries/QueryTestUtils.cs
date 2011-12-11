using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Queries.Companies;
using Moq;

namespace MyJobLeads.Tests.Queries
{
    public class QueryTestUtils
    {
        public static Mock<CompanyByIdQuery> GenerateCompanyByIdQueryMock()
        {
            var mock = new Mock<CompanyByIdQuery>(null, null);
            mock.Setup(x => x.RequestedByUserId(It.IsAny<int>())).Returns(mock.Object);
            mock.Setup(x => x.WithCompanyId(It.IsAny<int>())).Returns(mock.Object);
            return mock;
        }
    }
}
