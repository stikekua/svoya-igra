using SvoyaIgra.Btn.WSTestClient.Helpers;
using SvoyaIgra.Btn.WSTestClient.ViewModel.Base;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.Btn.WSTestClient.ViewModel
{
    public sealed class MainViewModel : ViewModelBase, IMainViewModel
    {
        // Logging support
        private static readonly ILog _log = LogManager.GetLogger(typeof(MainViewModel));

        private readonly IGlobalData _globalData;
        private bool IsConnect;

        public DelegateCommand ConnectCommand { get; set; }
        public DelegateCommand DisconnectCommand { get; set; }
        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand NextCommand { get; set; }
        public DelegateCommand ResetCommand { get; set; }

        public ObservableCollection<string> LogList { get; } = new ObservableCollection<string>();
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public string SelectedLog
        {
            get => _selectedLog;
            set { _selectedLog = value; OnPropertyChanged(); }
        }
        private string _selectedLog;

        public string NotificationText
        {
            get => _notificationText;
            set { _notificationText = value; OnPropertyChanged(); }
        }
        private string _notificationText;

        public MainViewModel(IGlobalData globalData)
        {
            _globalData = globalData;
            _globalData.WebSocketClient.Opened += Wss_Opened;
            _globalData.WebSocketClient.Closed += Wss_Closed;
            _globalData.WebSocketClient.Error += Wss_Error;
            _globalData.WebSocketClient.Received += Wss_NewMessage;

            IsConnect = false;

            ConnectCommand = new DelegateCommand(OnConnectButtonPressed);
            DisconnectCommand = new DelegateCommand(OnDisconnectButtonPressed);

            StartCommand = new DelegateCommand(OnStartButtonPressed);
            NextCommand = new DelegateCommand(OnNextButtonPressed);
            ResetCommand = new DelegateCommand(OnResetButtonPressed);

        }

        private void OnConnectButtonPressed(object obj)
        {
            if (_globalData.WebSocketClient.Connect())
            {
                _log.Info("WebSocketClient Connect");
                IsConnect = true;
            }
            else
            {
                _log.Error("WebSocketClient Error");
            }
        }
        private bool CanExecuteConnectButton(object obj)
        {
            return !IsConnect;
        }

        private void OnDisconnectButtonPressed(object obj)
        {
            _globalData.WebSocketClient.Dispose();
            IsConnect = false;
        }
        private bool CanExecuteDisconnectButton(object obj)
        {
            return IsConnect;
        }

        private void OnStartButtonPressed(object obj)
        {
            _log.Info("OnStartButtonPressed");
            if (_globalData.WebSocketClient.Send(WsMessages.StartCommand))
            {
                NotificationText += $"C: {WsMessages.StartCommand}\r\n";
                addToLogList($"C: {WsMessages.StartCommand}");
            }
        }

        private void OnNextButtonPressed(object obj)
        {
            _log.Info("OnNextButtonPressed");
            if (_globalData.WebSocketClient.Send(WsMessages.NextCommand))
            {
                NotificationText += $"C: {WsMessages.NextCommand}\r\n";
                addToLogList($"C: {WsMessages.NextCommand}");
            }
        }

        private void OnResetButtonPressed(object obj)
        {
            _log.Info("OnResetButtonPressed");
            if (_globalData.WebSocketClient.Send(WsMessages.ResetCommand))
            {
                NotificationText += $"C: {WsMessages.NextCommand}\r\n";
                addToLogList($"C: {WsMessages.NextCommand}");
            }
        }
        private bool Connected(object obj)
        {
            return IsConnect;
        }

        private void Wss_Opened()
        {
            _log.Info("Wss_Opened");
            NotificationText += $"Opened\r\n";
            addToLogList($"Opened");
        }

        private void Wss_Closed()
        {
            _log.Info("Wss_Closed");
            NotificationText += $"Closed\r\n";
            addToLogList($"Closed");
        }

        private void Wss_Error(string message)
        {
            _log.Info($"Wss_Error: {message}");
            NotificationText += $"Error {message}\r\n";
            addToLogList($"Error {message}");

        }
        private void Wss_NewMessage(string message)
        {
            _log.Info($"Wss_NewMessage: {message}");
            NotificationText += $"S: {message}\r\n";
            addToLogList($"S: {message}");
        }

        private void addToLogList(string message)
        {
            dispatcher.Invoke(() => LogList.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\t {message}"));
        }
    }
}
