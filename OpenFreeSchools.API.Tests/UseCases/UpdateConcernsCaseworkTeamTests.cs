using OpenFreeSchools.API.RequestModels.Concerns.TeamCasework;
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
    public class UpdateOpenFreeSchoolsTeamTests
    {
        [Fact]
        public async Task GetOpenFreeSchoolsTeam_Implements_IGetOpenFreeSchoolsTeam()
        {
            typeof(UpdateOpenFreeSchoolsTeam).Should().BeAssignableTo<IUpdateOpenFreeSchoolsTeam>();
        }

        [Fact]
        public async Task Execute_When_Team_Found_Performs_Update()
        {
            var ownerId = "john.doe";
            var mockGateway = new Mock<IConcernsTeamCaseworkGateway>();

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

            var updateRequest = new OpenFreeSchoolsTeamUpdateRequest()
            {
                OwnerId = ownerId,
                TeamMembers = new string[]
                {
                    "user.one",
                    "user.three"
                }
            };

            // Act
            var sut = new UpdateOpenFreeSchoolsTeam(mockGateway.Object);
            var result = await sut.Execute(updateRequest, CancellationToken.None);

            result.Should().NotBeNull();
            result.OwnerId.Should().Be(ownerId);
            result.TeamMembers.Length.Should().Be(2);
            result.TeamMembers.Should().Contain("user.one");
            result.TeamMembers.Should().Contain("user.three");

            mockGateway.Verify(x => x.UpdateCaseworkTeam(It.Is<OpenFreeSchoolsTeam>(c => c.Id == ownerId && c.TeamMembers.Count == 2), It.IsAny<CancellationToken>()), Times.Once);
            mockGateway.Verify(x => x.AddCaseworkTeam(It.IsAny<OpenFreeSchoolsTeam>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Execute_When_Team_Not_Found_Performs_Add()
        {
            var ownerId = "john.doe";
            var mockGateway = new Mock<IConcernsTeamCaseworkGateway>();

            mockGateway
            .Setup(g => g.GetByOwnerId(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(OpenFreeSchoolsTeam));

            var updateRequest = new OpenFreeSchoolsTeamUpdateRequest()
            {
                OwnerId = ownerId,
                TeamMembers = new string[]
                {
                    "user.one",
                    "user.three"
                }
            };

            // Act
            var sut = new UpdateOpenFreeSchoolsTeam(mockGateway.Object);
            var result = await sut.Execute(updateRequest, CancellationToken.None);

            result.Should().NotBeNull();
            result.OwnerId.Should().Be(ownerId);
            result.TeamMembers.Length.Should().Be(2);
            result.TeamMembers.Should().Contain("user.one");
            result.TeamMembers.Should().Contain("user.three");

            // verify add was called.
            mockGateway.Verify(x => x.UpdateCaseworkTeam(It.IsAny<OpenFreeSchoolsTeam>(), It.IsAny<CancellationToken>()), Times.Never);
            mockGateway.Verify(x => x.AddCaseworkTeam(It.Is<OpenFreeSchoolsTeam>(c => c.Id == ownerId && c.TeamMembers.Count == 2), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
