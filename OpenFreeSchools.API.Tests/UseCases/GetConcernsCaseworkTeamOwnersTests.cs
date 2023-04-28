using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases
{
    public class GetOpenFreeSchoolsTeamOwnersTests
    {
        [Fact]
        public async Task Execute_Calls_Gateway()
        {
            var expectedData = new[] { "user.1", "user.2", "user.3" };
            var mockGateway = new Mock<IConcernsTeamCaseworkGateway>();
            mockGateway.Setup(x => x.GetTeamOwners(It.IsAny<CancellationToken>())).ReturnsAsync(expectedData);
            var sut = new GetOpenFreeSchoolsTeamOwners(mockGateway.Object);

            var result = await sut.Execute(CancellationToken.None);
            result.Should().BeEquivalentTo(expectedData);
        }
    }
}
