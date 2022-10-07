using log4net;
using SvoyaIgra.Btn.ButtonClient.Helpers;
using SvoyaIgra.Btn.ButtonClient.ViewModel.Base;
using SvoyaIgra.WebSocketProvider.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace SvoyaIgra.Btn.ButtonClient.ViewModel;

public sealed class MainViewModel : ViewModelBase, IMainViewModel
{
    // Logging support
    private static readonly ILog _log = LogManager.GetLogger(typeof(MainViewModel));

    private readonly IGlobalData _globalData;

    private string _serverAddress;
    private bool _isConnect;

    public string ServerAddress
    {
        get => _serverAddress;
        set => SetProperty(ref _serverAddress, value);
    }

    public bool IsConnect
    {
        get => _isConnect;
        set
        {
            SetProperty(ref _isConnect, value);
            ConnectCommand?.OnCanExecuteChanged();
            DisconnectCommand?.OnCanExecuteChanged();
        }
    }

    public DelegateCommand ConnectCommand { get; set; }
    public DelegateCommand DisconnectCommand { get; set; }

    public ObservableCollection<string> LogList { get; private set; } = new ObservableCollection<string>();
    private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

    public DelegateCommand PlayerButtonCommand { get; set; }
    public DelegateCommand ClearButtonCommand { get; set; }


    public MainViewModel(IGlobalData globalData)
    {
        _globalData = globalData;

        ServerAddress = "ws://localhost:81";

        IsConnect = false;

        ConnectCommand = new DelegateCommand(OnConnectButtonPressed, CanConnectButtonPress);
        DisconnectCommand = new DelegateCommand(OnDisconnectButtonPressed, CanDisconnectButtonPress);

        PlayerButtonCommand = new DelegateCommand(OnPlayerButtonPressed);
        ClearButtonCommand = new DelegateCommand(OnClearButtonPressed);
    }

    private void OnClearButtonPressed(object obj)
    {
        LogList.Clear();
    }

    private void OnPlayerButtonPressed(object obj)
    {
        _log.Info($"ButtonPressed: {obj}");
        if (_globalData.WebSocketClient.Send($"{obj}"))
        {
            AddToLogList($"C: {obj}");
        }
    }

    private void OnConnectButtonPressed(object obj)
    {
        // WebSocketClient
        _globalData.WebSocketClient = new WebSocketClientProvider(ServerAddress);

        _globalData.WebSocketClient.Opened += Wss_Opened;
        _globalData.WebSocketClient.Closed += Wss_Closed;
        _globalData.WebSocketClient.Error += Wss_Error;
        _globalData.WebSocketClient.Received += Wss_NewMessage;

        _log.Info($"wsClient connecting to {ServerAddress}");
        AddToLogList($"wsClient connecting to {ServerAddress}");
        if (_globalData.WebSocketClient.Connect())
        {
            _log.Info("wsClient connected");
            AddToLogList($"Info wsClient connected");
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

    public bool CanDisconnectButtonPress(object obj)
    {
        return IsConnect;
    }

    public bool CanConnectButtonPress(object obj)
    {
        return !IsConnect;
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