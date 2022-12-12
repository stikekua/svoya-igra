using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                services.ConfigureAppOptions(_config);
                services.AddDiRegistration(_config);

                // Register all ViewModels.
                services.AddSingleton<MainViewModel>();
                //services.AddSingleton<ViewerViewModel>();
                //services.AddSingleton<EditorViewModel>();

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
