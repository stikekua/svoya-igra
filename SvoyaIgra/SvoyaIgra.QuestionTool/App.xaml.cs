using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SvoyaIgra.QuestionTool.Data;
using SvoyaIgra.QuestionTool.ViewModel;

namespace SvoyaIgra.QuestionTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IConfiguration _config;

        public static IServiceProvider ServiceProvider { get; private set; }

        private readonly IHost host;

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            _config = builder.Build();

            var connectionString = _config.GetConnectionString("SvoyaIgraDbContext");
            //if (string.IsNullOrWhiteSpace(connectionString))
            //{
            //    Ui.Error("Missing connection string for SvoyaIgraDbContext in appsettings.json");
            //}

            host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                // DbContexts
                services.AddDbContext<SvoyaIgraDbContext>(options => options.UseSqlServer(connectionString));

                // Di
                services.ConfigureAppOptions(_config);
                services.AddDiRegistration(_config);

                // Register all ViewModels.
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<AuthorsViewModel>();
                services.AddSingleton<TopicsViewModel>();
                services.AddSingleton<QuestionsViewModel>();
                services.AddSingleton<ImportViewModel>();

                // Register all the Windows of the applications.
                services.AddTransient<MainWindow>();
            }).Build();

            ServiceProvider = host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();
            base.OnExit(e);
        }
    }
}
