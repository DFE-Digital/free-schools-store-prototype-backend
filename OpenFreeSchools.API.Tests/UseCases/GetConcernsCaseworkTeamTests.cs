using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models.Concerns.TeamCasework;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases
{
    public class GetOpenFreeSchoolsTeamTests
    {
        [Fact]
        public async Task GetOpenFreeSchoolsTeam_Implements_IGetOpenFreeSchoolsTeam()
        {
            typeof(GetOpenFreeSchoolsTeam).Should().BeAssignableTo<GetOpenFreeSchoolsTeam>();
        }

        [Fact]
        public async Task Execute_When_Team_Found_Returns_OpenFreeSchoolsTeam()
        {
            var ownerId = "john.doe";
            var mockGateway = new Mock<IConcernsTeamCaseworkGateway>();
            var useCase = new GetOpenFreeSchoolsTeam(mockGateway.Object);

            mockGateway
            .Setup(g => g.GetByOwnerId(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OpenFreeSchoolsTeam
            {
                Id = ownerId,
                TeamMembers = new List<OpenFreeSchoolsTeamMember>
                {
                    new OpenFreeSchoolsTeamMember { TeamMember = "user.one" } ,
                    new OpenFreeSchoolsTeamMember { TeamMember = "user.two" } ,
                    new OpenFreeSchoolsTeamMember { TeamMember = "user.three" }
                }
            });

            var sut = new GetOpenFreeSchoolsTeam(mockGateway.Object);
            var result = await sut.Execute(ownerId, CancellationToken.None);

            result.Should().NotBeNull();
            result.OwnerId.Should().Be(ownerId);
            result.TeamMembers.Length.Should().Be(3);
            result.TeamMembers.Should().Contain("user.one");
            result.TeamMembers.Should().Contain("user.two");
            result.TeamMembers.Should().Contain("user.three");
        }

        [Fact]
        public async Task Execute_When_Team_NotFound_Returns_Null()
        {
            var ownerId = "john.doe";
            var mockGateway = new Mock<IConcernsTeamCaseworkGateway>();
            var useCase = new GetOpenFreeSchoolsTeam(mockGateway.Object);

            mockGateway
                .Setup(g => g.GetByOwnerId(ownerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(OpenFreeSchoolsTeam));

            var sut = new GetOpenFreeSchoolsTeam(mockGateway.Object);
            var result = await sut.Execute(ownerId, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
