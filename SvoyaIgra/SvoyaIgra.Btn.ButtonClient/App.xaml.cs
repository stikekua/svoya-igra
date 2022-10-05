using System;
using System.IO;
using System.Windows;
using log4net;
using log4net.Config;
using SvoyaIgra.Btn.ButtonClient.Helpers;
using SvoyaIgra.Btn.ButtonClient.ViewModel;
using SvoyaIgra.WebSocketProvider.Client;

namespace SvoyaIgra.Btn.ButtonClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(App));

        private IGlobalData _globalData;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Configure file logging log4net 
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs/log4net.config");
            XmlConfigurator.ConfigureAndWatch(new FileInfo(path));

            _log.Info("Starting application...");

            _globalData = new GlobalData();

            // WebSocketClient
            _globalData.WebSocketClient = new WebSocketClientProvider();

            var vm = new MainViewModel(_globalData);
            var window = new MainWindow();
            window.DataContext = vm;
            window.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _globalData.WebSocketClient?.Dispose();
            _globalData.CancellationToken.Cancel();
            base.OnExit(e);
        }
    }
}
