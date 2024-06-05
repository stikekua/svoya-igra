
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.MultimediaProvider.Stores;

namespace SvoyaIgra.QuestionTool;

static class OptionsConfiguration
{
    public static IServiceCollection ConfigureAppOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MultimediaStoreOptions>(config.GetSection("MultimediaStore"));
        return services;
    }
}