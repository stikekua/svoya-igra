using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.Dal.Services;
using SvoyaIgra.ImportCSV.Services;
using SvoyaIgra.TestClient.Actions;
using SvoyaIgra.TestClient.Data;

namespace SvoyaIgra.TestClient;

static class DiConfiguration
{
    public static IServiceCollection AddDiRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IAuthorService, AuthorService<SvoyaIgraDbContext>>();
        services.AddScoped<ITopicService, TopicService<SvoyaIgraDbContext>>();
        services.AddScoped<IQuestionService, QuestionService<SvoyaIgraDbContext>>();
        services.AddScoped<IGameService, GameService<SvoyaIgraDbContext>>();
        services.AddScoped<IImportService, ImportService>();
        services.AddTransient<IAuthorActions, AuthorActions>();
        services.AddTransient<ITopicActions, TopicActions>();
        services.AddTransient<IQuestionActions, QuestionActions>();
        services.AddTransient<IImportActions, ImportActions>();
        return services;
    }
}