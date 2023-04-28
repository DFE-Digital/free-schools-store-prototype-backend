using OpenFreeSchools.API.RequestModels.CaseActions.SRMA;
using OpenFreeSchools.API.ResponseModels.CaseActions.SRMA;
using OpenFreeSchools.API.UseCases.CaseActions.SRMA;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using SRMAStatus = OpenFreeSchools.Data.Enums.SRMAStatus;

namespace OpenFreeSchools.API.Tests.UseCases
{
    public class CreateSRMATests
	{
	    [Fact]
        public void CreateSRMA_ShouldCreateAndReturnSRMAResponse_WhenGivenCreateSRMARequest()
        {
			var status = SRMAStatus.Deployed;
			var dateOffered = DateTime.Now.AddDays(-5);
			var updatedAt = DateTime.Now;

			var createSRMARequest = Builder<CreateSRMARequest>
	            .CreateNew()
	            .With(r => r.Status = status)
	            .With(r => r.DateOffered = dateOffered)
	            .Build();

			var srmaDbModel = new SRMACase
			{
				StatusId = (int)status,
				DateOffered = dateOffered,
				UpdatedAt = updatedAt
            };

            var expectedResult = new SRMAResponse
            {
				DateOffered = dateOffered,
				Status = status,
				UpdatedAt = updatedAt
			};
			
			var mockGateway = new Mock<ISRMAGateway>();
            
            mockGateway.Setup(g => g.CreateSRMA(It.IsAny<SRMACase>())).Returns(Task.FromResult(srmaDbModel));
            
            var useCase = new CreateSRMA(mockGateway.Object);
            
            var result = useCase.Execute(createSRMARequest);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}