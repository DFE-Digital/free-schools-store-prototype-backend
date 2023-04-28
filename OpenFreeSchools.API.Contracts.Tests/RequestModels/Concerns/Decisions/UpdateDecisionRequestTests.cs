using AutoFixture;
using OpenFreeSchools.API.Contracts.Enums;
using OpenFreeSchools.API.Contracts.RequestModels.Concerns.Decisions;
using FluentAssertions;

namespace OpenFreeSchools.API.Tests.RequestModels.Concerns.Decisions
{
	public class UpdateDecisionRequestTests
	{
		[Fact]
		public void IsValid_When_Invalid_DecisionType_Returns_False()
		{
			var fixture = new Fixture();
			var sut = fixture.Build<UpdateDecisionRequest>()
				.With(x => x.DecisionTypes, new DecisionType[] { 0 })
				.Create();

			sut.IsValid().Should().BeFalse();
		}
	}
}
