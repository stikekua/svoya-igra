using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.Dal.Services;
using SvoyaIgra.TestClient.Data;
using SvoyaIgra.TestClient.Question;
using SvoyaIgra.TestClient.Theme;

namespace SvoyaIgra.TestClient;

static class DiConfiguration
{
    public static IServiceCollection AddDiRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IThemeService, ThemeService<SvoyaIgraDbContext>>();
        services.AddScoped<IQuestionService, QuestionService<SvoyaIgraDbContext>>();
        services.AddTransient<IThemeActions, ThemeActions>();
        services.AddTransient<IQuestionActions, QuestionActions>();
        return services;
    }
}