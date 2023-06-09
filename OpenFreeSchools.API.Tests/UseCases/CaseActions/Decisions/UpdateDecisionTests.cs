﻿using AutoFixture;
using OpenFreeSchools.API.Contracts.RequestModels.Concerns.Decisions;
using OpenFreeSchools.API.Contracts.ResponseModels.Concerns.Decisions;
using OpenFreeSchools.API.Factories.Concerns.Decisions;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.API.UseCases.CaseActions.Decisions;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models.Concerns.Case.Management.Actions.Decisions;
using OpenFreeSchools.Data.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases.CaseActions.Decisions
{
    public class UpdateDecisionTests
    {
        [Fact]
        public void UpdateDecisions_Is_Assignable_To_IUseCaseAsync()
        {
            var fixture = CreateFixture();
            var sut = fixture.Create<UpdateDecision>();

            sut.Should()
                .BeAssignableTo<IUseCaseAsync<(int urn, int decisionId, UpdateDecisionRequest details), UpdateDecisionResponse>>();
        }

        [Fact]
        public async Task Execute_When_ConcernsCase_Not_Found_Throws_Exception()
        {
            var mockDecisionFactory = new Mock<IDecisionFactory>();
            var mockUpdateDecisionResponseFactory = new Mock<IUpdateDecisionResponseFactory>();
            var mockGateWay = new Mock<IConcernsCaseGateway>();

            var fixture = CreateFixture(null, mockGateWay.Object, mockDecisionFactory.Object, mockUpdateDecisionResponseFactory.Object);
            var request = fixture.Create<UpdateDecisionRequest>();
            var caseUrn = fixture.Create<int>();
            var decisionId = fixture.Create<int>();

            mockGateWay.Setup(x => x.GetConcernsCaseByUrn(caseUrn, true)).Returns(default(ConcernsCase));

            var sut = fixture.Create<UpdateDecision>();
            Func<Task<UpdateDecisionResponse>> action = () => sut.Execute((caseUrn, decisionId, request), CancellationToken.None);

            (await action.Should().ThrowAsync<InvalidOperationException>()).And.Message.Should()
                .Be($"Concerns Case {caseUrn} not found");
            mockGateWay.Verify(x => x.GetConcernsCaseByUrn(caseUrn, true), Times.Once);
        }

        [Fact]
        public async Task Execute_When_ConcernsCase_Found_Updates_Case()
        {
            var mockDecisionFactory = new Mock<IDecisionFactory>();
            var mockUpdateDecisionResponseFactory = new Mock<IUpdateDecisionResponseFactory>();
            var mockGateWay = new Mock<IConcernsCaseGateway>();

            var fixture = CreateFixture(null, mockGateWay.Object, mockDecisionFactory.Object, mockUpdateDecisionResponseFactory.Object);
            var request = fixture.Create<UpdateDecisionRequest>();
            var caseUrn = fixture.Create<int>();
            var expectedDecision = fixture.Create<Decision>();
            var concernsCase = fixture.Create<ConcernsCase>();
            concernsCase.AddDecision(expectedDecision, DateTimeOffset.UtcNow);

            mockGateWay.Setup(x => x.GetConcernsCaseByUrn(caseUrn, true)).Returns(concernsCase);
            mockDecisionFactory.Setup(x => x.CreateDecision(It.IsAny<UpdateDecisionRequest>()))
                .Returns(expectedDecision);

            var sut = fixture.Create<UpdateDecision>();
            await sut.Execute((caseUrn, expectedDecision.DecisionId, request), CancellationToken.None);

            mockUpdateDecisionResponseFactory.Verify(x => x.Create(caseUrn, expectedDecision.DecisionId));
        }

        private Fixture CreateFixture(
            ILogger<UpdateDecision> logger = null,
            IConcernsCaseGateway gateway = null,
            IDecisionFactory decisionFactory = null,
            IUpdateDecisionResponseFactory updateDecisionResponseFactory = null)
        {
            var fixture = new Fixture();

            fixture.Register(() => logger ?? Mock.Of<ILogger<UpdateDecision>>());
            fixture.Register(() => gateway ?? Mock.Of<IConcernsCaseGateway>());
            fixture.Register(() => decisionFactory ?? Mock.Of<IDecisionFactory>());
            fixture.Register(() => updateDecisionResponseFactory ?? Mock.Of<IUpdateDecisionResponseFactory>());

            fixture.Register(() =>
            {
                var decision = Decision.CreateNew(
                    crmCaseNumber: new string(fixture.CreateMany<char>(Decision.MaxCaseNumberLength).ToArray()),
                    retrospectiveApproval: fixture.Create<bool>(),
                    submissionRequired: fixture.Create<bool>(),
                    submissionDocumentLink: new string(fixture.CreateMany<char>(Decision.MaxUrlLength).ToArray()),
                    receivedRequestDate: DateTimeOffset.Now,
                    decisionTypes: new DecisionType[] { new DecisionType(Data.Enums.Concerns.DecisionType.NoticeToImprove) },
                    totalAmountRequested: fixture.Create<decimal>(),
                    supportingNotes: new string(fixture.CreateMany<char>(Decision.MaxSupportingNotesLength).ToArray()),
                    DateTimeOffset.Now
                );
                decision.DecisionId = fixture.Create<int>();
                return decision;
            });
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}
