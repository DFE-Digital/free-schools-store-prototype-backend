namespace OpenFreeSchools.API.UseCases
{
    public interface IUseCase<in TRequest, out TResponse>
    {
        TResponse Execute(TRequest request);
    }
}
