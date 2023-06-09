﻿using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture;
using OpenFreeSchools.API.Factories.Concerns.Decisions;
using OpenFreeSchools.API.UseCases.CaseActions.Decisions;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models.Concerns.Case.Management.Actions.Decisions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases.CaseActions.Decisions
{
	    public class AllDecisionUseCasesTests
    {
        [Fact]
        public void All_Constructors_Guard_Against_Null()
        {
            // Arrange
            var requestTypes = GetTypes();
            var fixture = CreateFixture();

            foreach (var typeInfo in requestTypes)
            {
                using (var scope = new AssertionScope())
                {
                    scope.AddReportable("type", typeInfo.FullName);

                    // Act & Assert
                    var assertion = fixture.Create<GuardClauseAssertion>();

                    assertion.Verify(typeInfo.GetConstructors());
                }
            }
        }

        [Fact]
        public void All_Methods_Guard_Against_Null()
        {
			// Arrange
			var requestTypes = GetTypes();
            var fixture = CreateFixture();

            foreach (var typeInfo in requestTypes)
            {
                using (var scope = new AssertionScope())
                {
                    scope.AddReportable("type", typeInfo.FullName);

                    // Act & Assert
                    var assertion = fixture.Create<GuardClauseAssertion>();

                    assertion.Verify(typeInfo.GetMethods());
                }
            }
        }

        [Fact]
        public void Property_Setters_Work_As_Expected()
        {
            // Arrange
            var requestTypes = GetTypes();
            var fixture = CreateFixture();

            foreach (var typeInfo in requestTypes)
            {
                using (var scope = new AssertionScope())
                {
                    scope.AddReportable("type", typeInfo.FullName);

                    // Act & Assert
                    var assertion = fixture.Create<WritablePropertyAssertion>();

                    assertion.Verify(typeInfo.GetProperties());
                }
            }
        }

        private TypeInfo[] GetTypes()
        {
            // THe typeToFind is the key type used to find classes within the same namespace.
            var typeToFind = typeof(CreateDecision);
            return typeToFind.Assembly
                .DefinedTypes
                .Where(x =>
                    x.IsClass &&
                    x.GenericTypeParameters.Length == 0 &&
                    x.Namespace != null
                    && x.Namespace.Equals(typeToFind.Namespace))
					
                .ToArray();
        }

        private Fixture CreateFixture(
            ILogger<GetDecisions> logger = null,
            IConcernsCaseGateway gateway = null,
            IGetDecisionsSummariesFactory decisionsSummariesFactory = null)
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            fixture.Customize<Decision>(sb => sb.FromFactory(() =>
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
            }));

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}
