using log4net;
using SvoyaIgra.Btn.ButtonClient.Helpers;
using SvoyaIgra.WebSocketProvider.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SvoyaIgra.Btn.ButtonClient.ViewModel;

[INotifyPropertyChanged]
public partial class MainViewModel : IMainViewModel
{
    // Logging support
    private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));

    private readonly IGlobalData _globalData;

    [ObservableProperty]
    string _serverAddress;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
    [NotifyCanExecuteChangedFor(nameof(DisconnectCommand))]
    private bool _isConnect;
    
    public ObservableCollection<string> LogList { get; } = new ObservableCollection<string>();
    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
    
    public MainViewModel(IGlobalData globalData)
    {
        _globalData = globalData;

        ServerAddress = "ws://localhost:81";
        IsConnect = false;
        
    }

    [RelayCommand]
    private void Clear(object obj)
    {
        LogList.Clear();
    }

    [RelayCommand]
    private void PlayerButton(object obj)
    {
        Log.Info($"ButtonPressed: {obj}");
        if (_globalData.WebSocketClient != null)
        {
            if (_globalData.WebSocketClient.Send($"{obj}"))
            {
                AddToLogList($"C: {obj}");
            }
        }
    }

    [RelayCommand(CanExecute = nameof(CanConnect))]
    private void Connect(object obj)
    {
        // WebSocketClient
        _globalData.WebSocketClient = new WebSocketClientProvider(ServerAddress);

        _globalData.WebSocketClient.Opened += Wss_Opened;
        _globalData.WebSocketClient.Closed += Wss_Closed;
        _globalData.WebSocketClient.Error += Wss_Error;
        _globalData.WebSocketClient.Received += Wss_NewMessage;

        Log.Info($"wsClient connecting to {ServerAddress}");
        AddToLogList($"wsClient connecting to {ServerAddress}");
        if (_globalData.WebSocketClient.Connect())
        {
            Log.Info("wsClient connected");
            AddToLogList($"Info wsClient connected");
            IsConnect = true;
        }
        else
        {
            Log.Error("wsClient error");
            AddToLogList($"Error wsClient error");
        }
    }
    public bool CanConnect(object obj)
    {
        return !IsConnect;
    }

    [RelayCommand(CanExecute = nameof(CanDisconnect))]
    private void Disconnect(object obj)
    {
        Log.Info("WebSocketClient disconnecting");
        AddToLogList($"Info wsClient disconnecting");
        _globalData.WebSocketClient.Dispose();
        IsConnect = false;
    }

    public bool CanDisconnect(object obj)
    {
        return IsConnect;
    }
    
    #region WebSocket
    
    private void Wss_Opened()
    {
        Log.Info("Wss_Opened");
        AddToLogList($"Opened");
    }

    private void Wss_Closed()
    {
        Log.Info("Wss_Closed");
        AddToLogList($"Closed");
    }

    private void Wss_Error(string message)
    {
        Log.Info($"Wss_Error: {message}");
        AddToLogList($"Error {message}");

    }
    private void Wss_NewMessage(string message)
    {
        Log.Info($"Wss_NewMessage: {message}");
        AddToLogList($"S: {message}");
    }

    #endregion

    private void AddToLogList(string message)
    {
        _dispatcher.Invoke(() => LogList.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\t {message}"));
    }
}