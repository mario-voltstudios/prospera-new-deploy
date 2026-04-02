using ProsperaServices.Interfaces.IoC;

namespace ProsperaServices.Services;

public static class ApplicationServices
{
    extension(IServiceCollection services)
    {
        public void AddApplicationServices()
        {
            var assembly = typeof(ApplicationServices).Assembly;
            
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo<ISingletonDependency>())
                .AsImplementedInterfaces()
                .AsSelf()
                .WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo<IScopedDependency>())
                .AsImplementedInterfaces()
                .AsSelf()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ITransientDependency>())
                .AsImplementedInterfaces()
                .AsSelf()
                .WithTransientLifetime()
            );
        }
    }
}