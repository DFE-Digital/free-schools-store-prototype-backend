using OpenFreeSchools.API.Factories.CaseActionFactories;
using OpenFreeSchools.API.UseCases.CaseActions.SRMA;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases
{
    public class GetSRMAsByCaseIdTests
    {
        [Fact]
        public void GetSRMAByCaseId_ShouldReturnSRMAResponse_WhenGivenCaseId()
        {
            var caseId = 123;

            var matchingSRMA = new SRMACase
            {
                CaseUrn = caseId,
                Notes = "match"
            };

            var srmas = new List<SRMACase> {
                matchingSRMA,
                new SRMACase {
                    CaseUrn = 222,
                    Notes = "SRMA 1"
                },
                new SRMACase {
                    CaseUrn = 456,
                    Notes = "SRMA 2"
                }
            };

            var expectedResult = SRMAFactory.CreateResponse(matchingSRMA);

            var mockSRMAGateway = new Mock<ISRMAGateway>();
            mockSRMAGateway.Setup(g => g.GetSRMAsByCaseId(caseId)).Returns(Task.FromResult((ICollection<SRMACase>)srmas.Where(s => s.CaseUrn == caseId).ToList()));

            var useCase = new GetSRMAsByCaseId(mockSRMAGateway.Object);

            var result = useCase.Execute(caseId);

            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            result.First().Should().BeEquivalentTo(expectedResult);
        }
    }
}
