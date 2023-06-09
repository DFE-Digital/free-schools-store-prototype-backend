using OpenFreeSchools.API.Contracts.Enums;
using OpenFreeSchools.API.Factories;
using OpenFreeSchools.API.RequestModels;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using System;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases
{
    public class CreateConcernsCaseTests
    {
        [Fact]
        public void ShouldCreateAndReturnAConcernsCase_WhenGivenAConcernsCaseRequest()
        {
            var concernsCaseGateway = new Mock<IConcernsCaseGateway>();
            
            var createRequest = Builder<ConcernCaseRequest>.CreateNew()
                .With(c => c.CreatedAt = new DateTime(2022,10,13))
                .With(c => c.UpdatedAt = new DateTime(2022,06,07))
                .With(c => c.ReviewAt = new DateTime(2022,07,10))
                .With(c => c.CreatedBy = "7654")
                .With(c => c.Description = " Test Description for case")
                .With(c => c.CrmEnquiry = "3456")
                .With(c => c.TrustUkprn = "17654")
                .With(c => c.ReasonAtReview = "Test concerns")
                .With(c => c.DeEscalation = new DateTime(2022,04,01))
                .With(c => c.Issue = "Here is the issue")
                .With(c => c.CurrentStatus = "Case status")
                .With(c => c.CaseAim = "Here is the aim")
                .With(c => c.DeEscalationPoint = "Point of de-escalation")
                .With(c => c.NextSteps = "Here are the next steps")
                .With(c => c.CaseHistory = "Some case history")
                .With(c => c.DirectionOfTravel = "Up")
                .With(c => c.Territory = Territory.North_And_Utc__North_West)
                .With(c => c.StatusId = 2)
                .With(c => c.RatingId = 4)
                .With(c => c.TrustUkprn = "12345678")
                .Build();
            
            var createdConcernsCase = ConcernsCaseFactory.Create(createRequest);
            var expected = ConcernsCaseResponseFactory.Create(createdConcernsCase);
            
            concernsCaseGateway.Setup(g => g.SaveConcernsCase(It.IsAny<ConcernsCase>())).Returns(createdConcernsCase);

            var useCase = new CreateConcernsCase(concernsCaseGateway.Object);
            var result = useCase.Execute(createRequest);
            
            result.Should().BeEquivalentTo(expected);
        }
    }
}