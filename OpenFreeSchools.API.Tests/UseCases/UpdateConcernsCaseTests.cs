using OpenFreeSchools.API.Factories;
using OpenFreeSchools.API.RequestModels;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases
{
    public class UpdateConcernsCaseTests
    {
        [Fact]
        public void ShouldSaveAConcernsCase_WhenGivenAModelToUpdate()
        {
            var urn = 1234;
            var gateway = new Mock<IConcernsCaseGateway>();
            var concernsCase = Builder<ConcernsCase>.CreateNew().With(c => c.Urn = urn).Build();
            var updateRequest = Builder<ConcernCaseRequest>.CreateNew().Build();

            var concernsToUpdate = ConcernsCaseFactory.Update(concernsCase, updateRequest);

            gateway.Setup(g => g.GetConcernsCaseByUrn(urn, false)).Returns(concernsCase);
            gateway.Setup(g => g.Update(It.IsAny<ConcernsCase>())).Returns(concernsToUpdate);

            var expected = ConcernsCaseResponseFactory.Create(concernsToUpdate);

            var useCase = new UpdateConcernsCase(gateway.Object);
            var result = useCase.Execute(urn, updateRequest);

            result.Should().BeEquivalentTo(expected);
        }
    }
}