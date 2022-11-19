using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace SvoyaIgra.MultimediaProvider.Stores;

public static class MultimediaStoreServiceExtension
{
    public static IServiceCollection AddMultimediaStore(this IServiceCollection services, MultimediaStoreOptions options)
    {
        Directory.CreateDirectory(options.RootPath);
        var fileProvider = new PhysicalFileProvider(options.RootPath);
        services.AddSingleton<IFileProvider>(fileProvider);
        services.AddScoped<IMultimediaStore, MultimediaStore>();
        return services;
    }
}