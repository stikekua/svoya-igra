using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.ImportCSV.Services;
using SvoyaIgra.TestClient.Actions;
using SvoyaIgra.TestClient.Data;

namespace SvoyaIgra.TestClient;

class Program
{
    private static IConfiguration _config;
    private static IServiceProvider _serviceProvider;

    static void Main()
    {
        Ui.Write("Hello!");

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
                    var authorActions = _serviceProvider.GetService<IAuthorActions>();
                    authorActions.PerformAuthorAction();
                    break;
                case 2:
                    var topicActions = _serviceProvider.GetService<ITopicActions>();
                    topicActions.PerformTopicAction();
                    break;
                case 3:
                    var questionActions = _serviceProvider.GetService<IQuestionActions>();
                    questionActions.PerformQuestionAction();
                    break;
                case 4:
                    var importActions = _serviceProvider.GetService<IImportActions>();
                    importActions.PerformImportAction();
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
        Ui.Write("  1. Author");
        Ui.Write("  2. Topic");
        Ui.Write("  3. Question");
        Ui.Write("  4. Import");
        Ui.Write("  0. <- EXIT");
        return Ui.Choice(4);
    }
}

