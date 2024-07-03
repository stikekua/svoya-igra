using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.Dal.Services;
using SvoyaIgra.MultimediaProvider.Services;
using SvoyaIgra.MultimediaProvider.Stores;
using SvoyaIgra.MultimediaViewer.Data;

namespace SvoyaIgra.MultimediaViewer;

static class DiConfiguration
{
    public static IServiceCollection AddDiRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IMultimediaService, MultimediaService>();
        services.AddScoped<ITopicService, TopicService<SvoyaIgraDbContext>>();
        services.AddScoped<IQuestionService, QuestionService<SvoyaIgraDbContext>>();
        services.AddMultimediaStore(config.GetSection("MultimediaStore").Get<MultimediaStoreOptions>());
        return services;
    }
}