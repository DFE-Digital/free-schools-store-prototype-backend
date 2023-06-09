using AutoFixture;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases
{
	public class GetActiveConcernsCaseSummariesByOwnerTests
	{
		private readonly IFixture _fixture = new Fixture();
		[Fact]
		public async Task Execute_ShouldReturnListOfConcernsCaseResponsesForAGivenOwnerId()
		{
			// arrange
			var ownerId = _fixture.Create<string>();
			var gateway = new Mock<ICaseSummaryGateway>();
            
			var cases = Builder<ActiveCaseSummaryVm>.CreateListOfSize(10)
				.All()
				.With(c => c.CreatedBy = ownerId)
				.With(c => c.Rating = Builder<ConcernsRating>.CreateNew().Build())
				.Build();

			gateway.Setup(g => g.GetActiveCaseSummariesByOwner(ownerId)).ReturnsAsync(cases);
            
			var sut = new GetActiveConcernsCaseSummariesByOwner(gateway.Object);

			// act
			var result = await sut.Execute(ownerId);

			// assert
			result.Should().HaveCount(10);
		}

		[Fact]
		public async Task Execute_ShouldReturnEmptyListWhenNoConcernsCases()
		{
			// arrange
			var ownerId = _fixture.Create<string>();
			var gateway = new Mock<ICaseSummaryGateway>();

			var cases = new List<ActiveCaseSummaryVm>();

			gateway.Setup(g => g.GetActiveCaseSummariesByOwner(ownerId)).ReturnsAsync(cases);

			var sut = new GetActiveConcernsCaseSummariesByOwner(gateway.Object);
	        
			// act
			var result = await sut.Execute(ownerId);

			// assert
			result.Should().BeEmpty();
		}
	}
}