namespace MicroWebFramework.DI
{
    public interface IDependencyContainer
    {
        TService Resolve<TService, TImplementation>() where TImplementation : TService;
    }
}
