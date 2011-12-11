using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using MyJobLeads.DomainModel.Commands.Companies;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Tests.Commands
{
    public class CommandTestUtils
    {
        public static Mock<CreateCompanyCommand> GenerateCreateCompanyCommandMock()
        {
            var mock = new Mock<CreateCompanyCommand>(null);
            mock.Setup(x => x.CalledByUserId(It.IsAny<int>())).Returns(mock.Object);
            mock.Setup(x => x.SetCity(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.SetIndustry(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.SetMetroArea(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.SetName(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.SetNotes(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.SetPhone(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.SetState(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.SetZip(It.IsAny<string>())).Returns(mock.Object);
            mock.Setup(x => x.WithJobSearch(It.IsAny<int>())).Returns(mock.Object);
            mock.Setup(x => x.Execute()).Returns(new Company());

            return mock;
        }
    }
}
