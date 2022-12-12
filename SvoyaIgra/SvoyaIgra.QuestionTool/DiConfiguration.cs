using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.MultimediaProvider.Services;
using SvoyaIgra.MultimediaProvider.Stores;

namespace SvoyaIgra.QuestionTool;

static class DiConfiguration
{
    public static IServiceCollection AddDiRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IMultimediaService, MultimediaService>();
        services.AddMultimediaStore(config.GetSection("MultimediaStore").Get<MultimediaStoreOptions>());
        return services;
    }
}