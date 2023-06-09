﻿using AutoFixture;
using AutoFixture.Idioms;
using OpenFreeSchools.API.Contracts.RequestModels.Concerns.Decisions;
using OpenFreeSchools.API.Contracts.ResponseModels.Concerns.Decisions;
using OpenFreeSchools.API.Factories.Concerns.Decisions;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.API.UseCases.CaseActions.Decisions;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using OpenFreeSchools.Data.Models.Concerns.Case.Management.Actions.Decisions;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases.CaseActions.Decisions
{
	    public class CreateDecisionTests
    {
        [Fact]
        public void CreateDecision_Implements_IUseCaseAsync()
        {
            typeof(CreateDecision).Should().BeAssignableTo<IUseCaseAsync<CreateDecisionRequest, CreateDecisionResponse>>();
        }

        [Fact]
        public void Constructor_Guards_Against_Null_Arguments()
        {
            // Arrange
            var fixture = CreateFixture();
            fixture.Register<IConcernsCaseGateway>(() => Mock.Of<IConcernsCaseGateway>());
            fixture.Register<IDecisionFactory>(() => Mock.Of<IDecisionFactory>());
            fixture.Register<ICreateDecisionResponseFactory>(() => Mock.Of<ICreateDecisionResponseFactory>());
            var assertion = fixture.Create<GuardClauseAssertion>();

            // Act & Assert
            assertion.Verify(typeof(CreateDecision).GetConstructors());
        }

        [Fact]
        public async Task Execute_Throws_Exception_If_Request_IsNull()
        {
            var sut = new CreateDecision(Mock.Of<IConcernsCaseGateway>(), Mock.Of<IDecisionFactory>(), Mock.Of<ICreateDecisionResponseFactory>());

            Func<Task> action = async () => await sut.Execute(null, CancellationToken.None);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Execute_Throws_Exception_If_Request_IsInvalid()
        {
            var badRequestObject = new CreateDecisionRequest
            {
                DecisionTypes = new Contracts.Enums.DecisionType[] { 0 }
            };

            var sut = new CreateDecision(Mock.Of<IConcernsCaseGateway>(), Mock.Of<IDecisionFactory>(), Mock.Of<ICreateDecisionResponseFactory>());

            Func<Task> action = async () => await sut.Execute(badRequestObject, CancellationToken.None);
            (await action.Should().ThrowAsync<ArgumentException>()).And.ParamName.Should().Be("request");
        }

        [Fact]
        public async Task Execute_When_Concerns_Case_NotFound_Throws_Exception()
        {
            var fixture = CreateFixture();

            var mockGateway = new Mock<IConcernsCaseGateway>();
            mockGateway.Setup(x => x.GetConcernsCaseByUrn(It.IsAny<int>(), false))
                .Returns(default(ConcernsCase));

            var request = fixture.Create<CreateDecisionRequest>();

            var sut = new CreateDecision(mockGateway.Object, Mock.Of<IDecisionFactory>(), Mock.Of<ICreateDecisionResponseFactory>());
            Func<Task> action = async () => await sut.Execute(request, CancellationToken.None);

            (await action.Should().ThrowAsync<InvalidOperationException>()).And.Message.Should()
                .Contain($"The concerns case for urn {request.ConcernsCaseUrn}, was not found");
        }

        [Fact]
        public async Task Execute_Adds_Decision()
        {
            var fixture = CreateFixture();

            var fakeConcernsCase = fixture.Create<ConcernsCase>();

            var request = fixture.Create<CreateDecisionRequest>();
            var fakeNewDecision = CreateRandomDecision(fixture, request);

            var mockGateway = new Mock<IConcernsCaseGateway>();
            mockGateway.Setup(x => x.GetConcernsCaseByUrn(request.ConcernsCaseUrn, false))
                .Returns(fakeConcernsCase);

            var mockDecisionFactory = new Mock<IDecisionFactory>();
            mockDecisionFactory.Setup(x => x.CreateDecision(request))
                .Returns(fakeNewDecision);

            var fakeResponse = new CreateDecisionResponse(fakeConcernsCase.Urn, fixture.Create<int>());
            var mockResponseFactory = new Mock<ICreateDecisionResponseFactory>();
            mockResponseFactory.Setup(x => x.Create(fakeConcernsCase.Urn, fakeNewDecision.DecisionId))
                .Returns(fakeResponse);

            mockGateway.Setup(x => x.SaveConcernsCase(It.Is<ConcernsCase>(cc => cc.Decisions.First() == fakeNewDecision))).Returns(fakeConcernsCase);

            var sut = new CreateDecision(mockGateway.Object, mockDecisionFactory.Object, mockResponseFactory.Object);

            var result = await sut.Execute(request, CancellationToken.None);

            mockGateway.Verify(x => x.SaveConcernsCase(fakeConcernsCase), Times.Once);
            result.Should().Be(fakeResponse);
        }

        private static Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture;
        }

        private Decision CreateRandomDecision(Fixture fixture, CreateDecisionRequest request)
        {
            return Decision.CreateNew(
                crmCaseNumber: request.CrmCaseNumber,
                retrospectiveApproval: request.RetrospectiveApproval,
                submissionRequired: request.SubmissionRequired,
                submissionDocumentLink: request.SubmissionDocumentLink,
                receivedRequestDate: (DateTimeOffset)request.ReceivedRequestDate,
                decisionTypes: request.DecisionTypes.Select(x => new DecisionType((Data.Enums.Concerns.DecisionType)x)).ToArray(),
                totalAmountRequested: request.TotalAmountRequested,
                supportingNotes: request.SupportingNotes,
                fixture.Create<DateTimeOffset>());
        }
    }
}
