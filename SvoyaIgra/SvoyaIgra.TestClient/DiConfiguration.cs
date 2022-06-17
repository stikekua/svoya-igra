using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.Dal.Services;
using SvoyaIgra.TestClient.Data;
using SvoyaIgra.TestClient.Question;
using SvoyaIgra.TestClient.Topic;

namespace SvoyaIgra.TestClient;

static class DiConfiguration
{
    public static IServiceCollection AddDiRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ITopicService, TopicService<SvoyaIgraDbContext>>();
        services.AddScoped<IQuestionService, QuestionService<SvoyaIgraDbContext>>();
        services.AddTransient<ITopicActions, TopicActions>();
        services.AddTransient<IQuestionActions, QuestionActions>();
        return services;
    }
}