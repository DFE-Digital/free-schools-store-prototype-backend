﻿using Ardalis.GuardClauses;

namespace OpenFreeSchools.Logging;

public record CorrelationContext() : ICorrelationContext
{
	public string CorrelationId { get; private set; }
	public void SetContext(string correlationId)
	{
		this.CorrelationId = Guard.Against.NullOrWhiteSpace(correlationId);
	}

	public string HeaderKey { get => "x-correlation-id"; }
}