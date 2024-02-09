namespace MicroWebFramework.DI;

public static class DependencyServiceProvider
{
    public static List<DependencyServiceWrapper> services =
        new List<DependencyServiceWrapper>();

    private static Dictionary<Type, object> existingInstances =
        new Dictionary<Type, object>();
    public static void AddSingleton<TService, TImplementation>() where TImplementation : TService
    {
        services.Add(new DependencyServiceWrapper
        {
            TService = typeof(TService),
            TImplementation = typeof(TImplementation),
            LifeTime = ServiceLifeTime.Singleton
        });
    }
    public static void AddScoped<TService, TImplementation>() where TImplementation : TService
    {
        services.Add(new DependencyServiceWrapper
        {
            TService = typeof(TService),
            TImplementation = typeof(TImplementation),
            LifeTime = ServiceLifeTime.Scoped,
            ThreadId = Thread.CurrentThread.ManagedThreadId
        });
    }

    public static void AddTransient<TService, TImplementation>() where TImplementation : TService
    {
        services.Add(new DependencyServiceWrapper
        {
            TService = typeof(TService),
            TImplementation = typeof(TImplementation),
            LifeTime = ServiceLifeTime.Transient
        });
    }

    public static int NumberOfInstances = 0;
    public static object Get(Type TService)
    {
        var descriptor = services.FirstOrDefault(p => p.TService == TService);
        if (descriptor is null)
            throw new InvalidOperationException($"Service of type {TService.Name} is not registered.");
        if (NumberOfInstances is 0)
        {
            NumberOfInstances++;
            //if (descriptor.LifeTime is ServiceLifeTime.Singleton)
            //{
            existingInstances[TService] = Activator.CreateInstance(descriptor.TImplementation)!;
            //}
            return existingInstances[TService];
        }
        if (descriptor.LifeTime is ServiceLifeTime.Singleton)
            return existingInstances[TService];
        if (descriptor.LifeTime is ServiceLifeTime.Transient)
        {
            NumberOfInstances++;
            return Activator.CreateInstance(descriptor.TImplementation)!;
        }
        if (descriptor.LifeTime is ServiceLifeTime.Scoped &&
            descriptor.ThreadId!.Value == Thread.CurrentThread.ManagedThreadId)
            return existingInstances[TService];
        else
        {
            NumberOfInstances++;
            return Activator.CreateInstance(descriptor.TImplementation)!;
        }
    }
}

public class DependencyServiceWrapper
{
    public Type TService { get; set; }
    public Type TImplementation { get; set; }
    public ServiceLifeTime LifeTime { get; set; }
    public int? ThreadId { get; set; }
}

public enum ServiceLifeTime
{
    Singleton,
    Scoped,
    Transient
}
