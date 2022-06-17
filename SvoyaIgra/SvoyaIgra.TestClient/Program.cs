using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.TestClient.Data;
using SvoyaIgra.TestClient.Question;
using SvoyaIgra.TestClient.Topic;

namespace SvoyaIgra.TestClient;

class Program
{
    private static IConfiguration _config;
    private static IServiceProvider _serviceProvider;

    static void Main()
    {
        Ui.Write("Hello, World!");

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);
        _config = builder.Build();
        var serviceCollection = new ServiceCollection();

        var connectionString = _config.GetConnectionString("SvoyaIgraDbContext");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Ui.Error("Missing connection string for SvoyaIgraDbContext in appsettings.json");
        }

        serviceCollection.AddDbContext<SvoyaIgraDbContext>(options => options.UseSqlServer(connectionString));

        serviceCollection.AddDiRegistration(_config);
        serviceCollection.AddSingleton(_config);
        _serviceProvider = serviceCollection.BuildServiceProvider();
        
        do
        {
            var index = Menu();
            switch (index)
            {
                case 0:
                    return;
                case 1:
                    var topicActions = _serviceProvider.GetService<ITopicActions>();
                    topicActions.PerformTopicAction();
                    break;
                case 2:
                    var questionActions = _serviceProvider.GetService<IQuestionActions>();
                    questionActions.PerformQuestionAction();
                    break;
                default:
                    continue;
            }
        } while (true);
        
    }

    private static int Menu()
    {
        Ui.Clear();
        Ui.Write("Select action:");
        Ui.Write("  1. Topic");
        Ui.Write("  2. Question");
        Ui.Write("  0. <- EXIT");
        return Ui.Choice(2);
    }
}

