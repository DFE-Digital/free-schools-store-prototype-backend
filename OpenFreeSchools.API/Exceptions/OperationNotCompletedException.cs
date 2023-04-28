namespace OpenFreeSchools.API.Exceptions;

public class OperationNotCompletedException : Exception
{
	public OperationNotCompletedException(string message) : base(message)
	{

	}
}