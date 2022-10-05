using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using log4net;
using SvoyaIgra.Btn.ButtonClient.ViewModel.Base;
using SvoyaIgra.Btn.ButtonClient.Helpers;
using SvoyaIgra.WebSocketProvider.Client;

namespace SvoyaIgra.Btn.ButtonClient.ViewModel;

public class MainViewModel : ViewModelBase, IMainViewModel
{
    // Logging support
    private static readonly ILog _log = LogManager.GetLogger(typeof(MainViewModel));

    private readonly IGlobalData _globalData;
    private bool IsConnect;

    public DelegateCommand ConnectCommand { get; set; }
    public DelegateCommand DisconnectCommand { get; set; }

    public ObservableCollection<string> LogList { get; } = new ObservableCollection<string>();
    private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

    public DelegateCommand PlayerButtonCommand { get; set; }


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

        PlayerButtonCommand = new DelegateCommand(OnPlayerButtonPressed);
    }

    private void OnPlayerButtonPressed(object obj)
    {
        _log.Info("ButtonPressed: RED");
        if (_globalData.WebSocketClient.Send("RED"))
        {
            AddToLogList($"C: RED");
        }
    }

    private void OnConnectButtonPressed(object obj)
    {
        if (_globalData.WebSocketClient.Connect())
        {
            _log.Info("wsClient connecting");
            AddToLogList($"Info wsClient connecting");
            IsConnect = true;
        }
        else
        {
            _log.Error("wsClient error");
            AddToLogList($"Error wsClient error");
        }
    }
    private void OnDisconnectButtonPressed(object obj)
    {
        _log.Info("WebSocketClient disconnecting");
        AddToLogList($"Info wsClient disconnecting");
        _globalData.WebSocketClient.Dispose();
        IsConnect = false;
    }

    private void Wss_Opened()
    {
        _log.Info("Wss_Opened");
        AddToLogList($"Opened");
    }

    private void Wss_Closed()
    {
        _log.Info("Wss_Closed");
        AddToLogList($"Closed");
    }

    private void Wss_Error(string message)
    {
        _log.Info($"Wss_Error: {message}");
        AddToLogList($"Error {message}");

    }
    private void Wss_NewMessage(string message)
    {
        _log.Info($"Wss_NewMessage: {message}");
        AddToLogList($"S: {message}");
    }

    private void AddToLogList(string message)
    {
        dispatcher.Invoke(() => LogList.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\t {message}"));
    }
}