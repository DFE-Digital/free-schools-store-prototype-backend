using OpenFreeSchools.API.RequestModels.CaseActions.FinancialPlan;
using OpenFreeSchools.API.ResponseModels.CaseActions.FinancialPlan;
using OpenFreeSchools.API.UseCases.CaseActions.FinancialPlan;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases
{
    public class CreateFinancialPlanTests
	{
	    [Fact]
        public void CreateFinancialPlan_ShouldCreateAndReturnFinancialPlanResponse_WhenGivenCreateFinancialPlanRequest()
        {
			var caseUrn = 743;
			var now = DateTime.Now;
			var statusId = 2;

			var createFPRequest = Builder<CreateFinancialPlanRequest>
	            .CreateNew()
	            .With(r => r.CaseUrn = caseUrn)
	            .With(r => r.CreatedAt = now)
	            .Build();

			var fpDbModel = new FinancialPlanCase
			{
				CaseUrn = caseUrn,
				CreatedAt = now,
				StatusId = statusId
            };

            var expectedResult = new FinancialPlanResponse
            {
				CaseUrn = caseUrn,
				CreatedAt = now,
				StatusId = statusId
			};
			
			var mockGateway = new Mock<IFinancialPlanGateway>();
            
            mockGateway.Setup(g => g.CreateFinancialPlan(It.IsAny<FinancialPlanCase>())).Returns(Task.FromResult(fpDbModel));
            
            var useCase = new CreateFinancialPlan(mockGateway.Object);
            
            var result = useCase.Execute(createFPRequest);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}