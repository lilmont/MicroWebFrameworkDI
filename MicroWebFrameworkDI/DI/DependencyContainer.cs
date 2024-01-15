namespace MicroWebFramework.DI
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly Dictionary<Type, Type> _registrations;

        public DependencyContainer()
        {
            _registrations = new Dictionary<Type, Type>();
        }

        public TService Resolve<TService, TImplementation>() where TImplementation : TService
        {
            _registrations[typeof(TService)] = typeof(TImplementation);
            return (TService)Resolve(typeof(TService));
        }

        private object Resolve(Type serviceType)
        {
            if (!_registrations.TryGetValue(serviceType, out var implementationType))
            {
                throw new InvalidOperationException($"Service of type {serviceType.Name} is not registered.");
            }
            var constructorInfo = implementationType.GetConstructors().First();
            return Activator.CreateInstance(implementationType);
        }
    }
}
