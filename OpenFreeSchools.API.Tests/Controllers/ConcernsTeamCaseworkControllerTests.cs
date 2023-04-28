using OpenFreeSchools.API.Controllers;
using OpenFreeSchools.API.RequestModels.Concerns.TeamCasework;
using OpenFreeSchools.API.ResponseModels;
using OpenFreeSchools.API.ResponseModels.Concerns.TeamCasework;
using OpenFreeSchools.API.UseCases;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.Controllers
{
    public class ConcernsTeamCaseworkControllerTests
    {
        private readonly Mock<ILogger<ConcernsTeamCaseworkController>> _mockLogger = new Mock<ILogger<ConcernsTeamCaseworkController>>();

        [Fact]
        public async Task Get_Returns200_When_Successfully_Fetched_Data()
        {
            // arrange
            var expectedOwnerId = "john.smith";
            var expectedData = new OpenFreeSchoolsTeamResponse() { OwnerId = expectedOwnerId, TeamMembers = new[] { "john.doe", "jane.doe", "fred.flintstone" } };
            
            var getTeamCommand = new Mock<IGetOpenFreeSchoolsTeam>();
            getTeamCommand.Setup(x => x.Execute(expectedOwnerId, It.IsAny<CancellationToken>())).ReturnsAsync(expectedData);

            var getTeamOwnersCommand = Mock.Of<IGetOpenFreeSchoolsTeamOwners>();
            var updateCommand = Mock.Of<IUpdateOpenFreeSchoolsTeam>();
            var getOwnersOfOpenCasesCommand = Mock.Of<IGetOwnersOfOpenCases>();

            var controller = new ConcernsTeamCaseworkController(
                _mockLogger.Object,
                getTeamCommand.Object,
                getTeamOwnersCommand,
                updateCommand,
                getOwnersOfOpenCasesCommand
            );

            // act
            var actionResult = await controller.GetTeam("john.smith", CancellationToken.None);
            var expectedResponse = new ApiSingleResponseV2<OpenFreeSchoolsTeamResponse>(expectedData);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okResult.StatusCode.Value.Should().Be(StatusCodes.Status200OK);
            (okResult.Value as ApiSingleResponseV2<OpenFreeSchoolsTeamResponse>).Should().NotBeNull();
            ((ApiSingleResponseV2<OpenFreeSchoolsTeamResponse>)okResult.Value).Data.Should().BeEquivalentTo(expectedData);
        }


        [Fact]
        public async Task Get_ReturnsNoContent_When_No_Data_Available()
        {
            // arrange
            var expectedOwnerId = "john.smith";
            var expectedData = new OpenFreeSchoolsTeamResponse() { OwnerId = expectedOwnerId, TeamMembers = new[] { "john.doe", "jane.doe", "fred.flintstone" } };

            var getTeamCommand = new Mock<IGetOpenFreeSchoolsTeam>();
            getTeamCommand.Setup(x => x.Execute(expectedOwnerId, It.IsAny<CancellationToken>())).ReturnsAsync(default(OpenFreeSchoolsTeamResponse));

            var getTeamOwnersCommand = new Mock<IGetOpenFreeSchoolsTeamOwners>();

            var updateCommand = new Mock<IUpdateOpenFreeSchoolsTeam>();
            var getOwnersOfOpenCasesCommand = Mock.Of<IGetOwnersOfOpenCases>();

            var controller = new ConcernsTeamCaseworkController(
                _mockLogger.Object,
                getTeamCommand.Object,
                getTeamOwnersCommand.Object,
                updateCommand.Object,
                getOwnersOfOpenCasesCommand
            );

            // act
            var actionResult = await controller.GetTeam("john.smith", CancellationToken.None);
            Assert.IsType<NoContentResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetTeamOwners_Returns_200_And_Data_When_Data_Exists()
        {
            // arrange
            var expectedData = new[] { "john.doe", "jane.doe", "fred.flintstone" };

            var getTeamOwnersCommand = new Mock<IGetOpenFreeSchoolsTeamOwners>();
            getTeamOwnersCommand.Setup(x => x.Execute(CancellationToken.None)).ReturnsAsync(expectedData);
            
            var updateCommand = new Mock<IUpdateOpenFreeSchoolsTeam>();
            var getOwnersOfOpenCasesCommand = Mock.Of<IGetOwnersOfOpenCases>();

            var controller = new ConcernsTeamCaseworkController(
                _mockLogger.Object,
                Mock.Of<IGetOpenFreeSchoolsTeam>(),
                getTeamOwnersCommand.Object,
                updateCommand.Object,
                getOwnersOfOpenCasesCommand
            );

            // act
            var actionResult = await controller.GetTeamOwners(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okResult.StatusCode.Value.Should().Be(StatusCodes.Status200OK);
            (okResult.Value as ApiSingleResponseV2<string[]>).Should().NotBeNull();
            ((ApiSingleResponseV2<string[]>)okResult.Value).Data.Should().BeEquivalentTo(expectedData);
        }

        [Fact]
        public async Task GetTeamOwners_Returns_200_When_No_Data_Exists()
        {
            // arrange
            var getTeamOwnersCommand = new Mock<IGetOpenFreeSchoolsTeamOwners>();
            getTeamOwnersCommand.Setup(x => x.Execute(CancellationToken.None)).ReturnsAsync(default(string[]));

            var updateCommand = new Mock<IUpdateOpenFreeSchoolsTeam>();
            var getOwnersOfOpenCasesCommand = Mock.Of<IGetOwnersOfOpenCases>();

            var controller = new ConcernsTeamCaseworkController(
                _mockLogger.Object,
                Mock.Of<IGetOpenFreeSchoolsTeam>(),
                getTeamOwnersCommand.Object,
                updateCommand.Object,
                getOwnersOfOpenCasesCommand
            );

            // act
            var actionResult = await controller.GetTeamOwners(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okResult.StatusCode.Value.Should().Be(StatusCodes.Status200OK);
            (okResult.Value as ApiSingleResponseV2<string[]>).Should().NotBeNull();
            ((ApiSingleResponseV2<string[]>)okResult.Value).Data.Should().BeEquivalentTo(Array.Empty<string>());
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_When_OwnerId_Differs_From_Model()
        {
            // arrange            
            var getTeamCommand = new Mock<IGetOpenFreeSchoolsTeam>();
            var updateTeamCommand = new Mock<IUpdateOpenFreeSchoolsTeam>();
            var getTeamOwnersCommand = new Mock<IGetOpenFreeSchoolsTeamOwners>();
            var getOwnersOfOpenCasesCommand = Mock.Of<IGetOwnersOfOpenCases>();

            var controller = new ConcernsTeamCaseworkController(
                _mockLogger.Object,
                getTeamCommand.Object,
                getTeamOwnersCommand.Object,
                updateTeamCommand.Object,
                getOwnersOfOpenCasesCommand                
            );

            var updateModel = new OpenFreeSchoolsTeamUpdateRequest
            {
                OwnerId = "different.ownerId",
                TeamMembers = new[] { "Barny.Rubble" }
            };

            // act
            var actionResult = await controller.Put("john.smith", updateModel, CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_When_Model_IsNull()
        {
            // arrange
            var getTeamCommand = new Mock<IGetOpenFreeSchoolsTeam>();
            var updateTeamCommand = new Mock<IUpdateOpenFreeSchoolsTeam>();
            var getTeamOwnersCommand = new Mock<IGetOpenFreeSchoolsTeamOwners>();
            var getOwnersOfOpenCasesCommand = Mock.Of<IGetOwnersOfOpenCases>();

            var controller = new ConcernsTeamCaseworkController(
                _mockLogger.Object,
                getTeamCommand.Object,
                getTeamOwnersCommand.Object,
                updateTeamCommand.Object,
                getOwnersOfOpenCasesCommand
            );

            // act
            var actionResult = await controller.Put("john.smith", null, CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task Put_ReturnsOK_When_UpdateCommand_Executed()
        {
            // arrange
            var expectedOwnerId = "john.smith";
            var expectedModel = new OpenFreeSchoolsTeamUpdateRequest() { OwnerId = expectedOwnerId, TeamMembers = new[] { "john.doe", "jane.doe", "fred.flintstone" } };

            var getTeamCommand = new Mock<IGetOpenFreeSchoolsTeam>();
            var updateTeamCommand = new Mock<IUpdateOpenFreeSchoolsTeam>();
            var getTeamOwnersCommand = new Mock<IGetOpenFreeSchoolsTeamOwners>();
            var getOwnersOfOpenCasesCommand = Mock.Of<IGetOwnersOfOpenCases>();

            updateTeamCommand.Setup(x => x.Execute(expectedModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OpenFreeSchoolsTeamResponse { OwnerId = expectedModel.OwnerId, TeamMembers = expectedModel.TeamMembers });

            var controller = new ConcernsTeamCaseworkController(
                _mockLogger.Object,
                getTeamCommand.Object,
                getTeamOwnersCommand.Object,
                updateTeamCommand.Object,
                getOwnersOfOpenCasesCommand
            );

            // act
            var actionResult = await controller.Put(expectedOwnerId, expectedModel, CancellationToken.None);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            (okResult.Value as ApiSingleResponseV2<OpenFreeSchoolsTeamResponse>).Should().NotBeNull();
            ((ApiSingleResponseV2<OpenFreeSchoolsTeamResponse>)okResult.Value).Data.Should().BeEquivalentTo(expectedModel);
        }
    }
}
