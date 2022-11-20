using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.Dal.Services;
using SvoyaIgra.Game.Data;
using SvoyaIgra.MultimediaProvider.Services;
using SvoyaIgra.MultimediaProvider.Stores;

namespace SvoyaIgra.Game;

static class DiConfiguration
{
    public static IServiceCollection AddDiRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IAuthorService, AuthorService<SvoyaIgraDbContext>>();
        services.AddScoped<ITopicService, TopicService<SvoyaIgraDbContext>>();
        services.AddScoped<IQuestionService, QuestionService<SvoyaIgraDbContext>>();
        services.AddScoped<IGameService, GameService<SvoyaIgraDbContext>>();

        services.AddScoped<IMultimediaService, MultimediaService>();
        services.AddMultimediaStore(config.GetSection("MultimediaStore").Get<MultimediaStoreOptions>());
        return services;
    }
}