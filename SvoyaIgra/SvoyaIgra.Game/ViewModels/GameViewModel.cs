﻿using SvoyaIgra.Game.Metadata;
using SvoyaIgra.Game.ViewModels.Helpers;
using SvoyaIgra.Game.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.IO;
using SvoyaIgra.WebSocketProvider.Client;
using SvoyaIgra.Shared.Constants;
using SvoyaIgra.MultimediaProvider.Services;
using System.Windows.Media;
using SvoyaIgra.Shared.Entities;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SvoyaIgra.Game.Enums;
using SvoyaIgra.Game.Helpers;
using Topic = SvoyaIgra.Game.Metadata.Topic;
using Question = SvoyaIgra.Game.Metadata.Question;
using System.Media;
using SvoyaIgra.Game.Views.Questions;

namespace SvoyaIgra.Game.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public WindowLocator WindowLocator { get; set; }
        private readonly IMultimediaService _multimediaService;

        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        #region Properties

        #region Buttons

        #region WS

        public WebSocketClientProvider WebSocketClient { get; set; }

        string _buttonServerAddress = "ws://localhost:81"; 
        public string ButtonServerAddress
        {
            get { return _buttonServerAddress; }
            set
            {
                if (_buttonServerAddress != value)
                {
                    _buttonServerAddress = value;
                    OnPropertyChanged(nameof(ButtonServerAddress));
                }
            }
        }
        
        ObservableCollection<string> _wsLogOC = new ObservableCollection<string>();
        public ObservableCollection<string> WsLogOC
        {
            get { return _wsLogOC; }
            set
            {
                if (_wsLogOC != value)
                {
                    _wsLogOC = value;
                    OnPropertyChanged(nameof(WsLogOC));
                    OnPropertyChanged(nameof(WsLogSelectedIndex));
                }
            }
        }

        public int WsLogSelectedIndex =>  WsLogOC.Count > 0 ? WsLogOC.Count - 1 : -1;

        private string _buttonsConnectionStatus = BtnsConnectionStatus.Unknown;
        public string ButtonsConnectionStatus
        {
            get { return _buttonsConnectionStatus; }
            set
            {
                if (_buttonsConnectionStatus != value)
                {
                    _buttonsConnectionStatus = value;
                    OnPropertyChanged(nameof(ButtonsConnectionStatus));
                }
            }
        }

        string _buttonsMessageText = ButtonMessageDecoder.EmptyMessage;
        public string ButtonsMessageText
        {
            get { return _buttonsMessageText; }
            set
            {
                if (_buttonsMessageText != value)
                {
                    _buttonsMessageText = value;
                    OnPropertyChanged(nameof(ButtonsMessageText));

                    if (PlayerSelectionMode==PlayerSelectionModeEnum.Auto)
                    {
                        SelectPlayerMethod((int)ButtonMessageDecoder.GetSelectedPlayerIndex(ButtonsMessageText));
                        PlayerInQueueDetector(ButtonsMessageText);
                    }
                }
            }
        }

        public RelayCommand ConnectButtonsServerCommand { get; set; }
        public RelayCommand DisconnectButtonsServerCommand { get; set; }
        public RelayCommand RequestNextPlayerCommand { get; set; }
        public RelayCommand ResetButtonsStateCommand { get; set; }
        public RelayCommand ClearWsLogCommand { get; set; }
        #endregion
        
        PlayerSelectionModeEnum _playerSelectionMode = PlayerSelectionModeEnum.Auto;
        public PlayerSelectionModeEnum PlayerSelectionMode
        {
            get { return _playerSelectionMode; }
            set
            {
                if (_playerSelectionMode != value)
                {
                    _playerSelectionMode = value;
                    OnPropertyChanged(nameof(PlayerSelectionMode));

                    if (_playerSelectionMode == PlayerSelectionModeEnum.Manual)
                    {
                        ButtonsMessageText = ButtonMessageDecoder.EmptyMessage;
                        SelectPlayerMethod(-1);
                    }

                }
            }
        }

        #region Buttons control

        bool _readyToCollectAnswers = false;
        public bool ReadyToCollectAnswers
        {
            get { return _readyToCollectAnswers; }
            set
            {
                if (_readyToCollectAnswers != value)
                {
                    _readyToCollectAnswers = value;
                    OnPropertyChanged(nameof(ReadyToCollectAnswers));
                    if (value && ButtonsConnectionStatus == BtnsConnectionStatus.Connected) ResetButtonsStateMethod(null);
                }
            }
        }
        
        #endregion

        public RelayCommand SetReadyForAnswersCommand { get; set; }
        #endregion                

        #region Cockpit window control

        public RelayCommand CloseAppCommand { get; set; }
        
        #endregion

        #region PlayScreen

        PlayScreenWindow _playScreenWindow;
        public PlayScreenWindow PlayScreenWindow 
        { 
            get { return _playScreenWindow; }
            set
            {
                if (_playScreenWindow!=value)
                {
                    _playScreenWindow = value;
                    OnPropertyChanged(nameof(PlayScreenWindow));
                    OnPropertyChanged(nameof(PlayScreenRunning));
                }
            }
        }

        private WindowState _playScreenWindowState = WindowState.Normal;
        public WindowState PlayScreenWindowState
        {
            get { return _playScreenWindowState; }
            set
            {
                if (_playScreenWindowState != value)
                {
                    _playScreenWindowState = value;
                    OnPropertyChanged(nameof(PlayScreenWindowState));
                }
            }
        }
        
        public bool PlayScreenRunning => PlayScreenWindow != null;

        bool _playScreenLocked;
        public bool PlayScreenLocked
        {
            get { return _playScreenLocked; }
            set
            {
                if (_playScreenLocked != value)
                {
                    _playScreenLocked = value;
                    OnPropertyChanged(nameof(PlayScreenLocked));
                    LockPresentScreenMethod(value);
                }
            }
        }
        
        public RelayCommand OpenPresentScreenCommand { get; set; } 
        public RelayCommand ClosePresentScreenCommand { get; set; } 
        public RelayCommand ChangeGamePhaseCommand { get; set; }
        
        #endregion

        #region Game phase

        private int _gamePhase = (int)GamePhaseEnum.PreGame;
        public int GamePhase
        {
            get { return _gamePhase; }
            set
            {
                if (_gamePhase == (int)GamePhaseEnum.FirstRound || _gamePhase == (int)GamePhaseEnum.SecondRound || _gamePhase == (int)GamePhaseEnum.ThirdRound)
                    ActualRoundGamePhase = _gamePhase;

                if (_gamePhase != value)
                {
                    _gamePhase = value;
                    OnPropertyChanged(nameof(GamePhase));
                    OnPropertyChanged(nameof(IsIntroOnScreen));
                    OnPropertyChanged(nameof(IsQuestion));
                    
                    GamePhaseUpdate();
                }
            }
        }
        public bool IsIntroOnScreen
        {
            get {
                return (GamePhase == (int)GamePhaseEnum.GameIntro)
                       || (GamePhase == (int)GamePhaseEnum.FirstRoundIntro)
                       || (GamePhase == (int)GamePhaseEnum.SecondRoundIntro)
                       || (GamePhase == (int)GamePhaseEnum.ThirdRoundIntro)
                       || (GamePhase == (int)GamePhaseEnum.FinalRoundIntro);
            }
        }

        private int _actualRoundGamePhase = 0;
        public int ActualRoundGamePhase
        {
            get { return _actualRoundGamePhase; }
            set
            {
                if (_actualRoundGamePhase != value)
                {
                    _actualRoundGamePhase = value;
                    OnPropertyChanged(nameof(ActualRoundGamePhase));
                }
            }
        }
        #endregion

        #region Questions

        private ObservableCollection<ObservableCollection<Topic>> _allRoundsQuestions { get; set; } = new ObservableCollection<ObservableCollection<Topic>>();
        public ObservableCollection<ObservableCollection<Topic>> AllRoundsQuestions
        {
            get { return _allRoundsQuestions; }
            set
            {
                if (_allRoundsQuestions != value)
                {
                    _allRoundsQuestions = value;
                    OnPropertyChanged(nameof(AllRoundsQuestions));
                    OnPropertyChanged(nameof(QuestionsArePrepared));
                }
            }
        }


        private ObservableCollection<Topic> _currentRoundQuestions;
        public ObservableCollection<Topic> CurrentRoundQuestions
        {
            get { return _currentRoundQuestions; }
            set
            {
                if (_currentRoundQuestions != value)
                {
                    _currentRoundQuestions = value;
                    OnPropertyChanged(nameof(CurrentRoundQuestions));
                }
            }
        }

        public bool QuestionsArePrepared => AllRoundsQuestions.Count > 0;

        private Question _currentQuestion = new Question();
        public Question CurrentQuestion 
        {   
            get {  return _currentQuestion; }
            set 
            {
                if (_currentQuestion!=value)
                {                    
                    _currentQuestion = value;
                    OnPropertyChanged(nameof(CurrentQuestion));
                    OnPropertyChanged(nameof(IsPictureQuestion));
                    OnPropertyChanged(nameof(IsPictureSeriesQuestion));
                    OnPropertyChanged(nameof(IsMusicQuestion));
                    OnPropertyChanged(nameof(IsVideoQuestion));
                } 
            } 
        }
        public bool IsQuestion => GamePhase == (int)GamePhaseEnum.Question;

        public bool IsPictureQuestion => CurrentQuestion.QuestionType == QuestionTypeEnum.Picture;
        public bool IsPictureSeriesQuestion => CurrentQuestion.QuestionType == QuestionTypeEnum.PictureSeries;
        public bool IsMusicQuestion => CurrentQuestion.QuestionType == QuestionTypeEnum.Musical;
        public bool IsVideoQuestion => CurrentQuestion.QuestionType == QuestionTypeEnum.Video;


        private Question _finalQuestion = new Question();
        public Question FinalQuestion
        {
            get { return _finalQuestion; }
            set
            {
                if (_finalQuestion != value)
                {
                    _finalQuestion = value;
                    OnPropertyChanged(nameof(FinalQuestion));
                }
            }
        }

        private ImageSource _imageSourceQuestion;
        public ImageSource ImageSourceQuestion
        {
            get { return _imageSourceQuestion; }
            set
            {
                if (_imageSourceQuestion != value)
                {
                    _imageSourceQuestion = value;
                    OnPropertyChanged(nameof(ImageSourceQuestion));
                }
            }
        }

        private ObservableCollection<BitmapImage> _pictureSeriesBitmaps;
        public ObservableCollection<BitmapImage> PictureSeriesBitmaps
        {
            get { return _pictureSeriesBitmaps; }
            set
            {
                if (_pictureSeriesBitmaps != value)
                {
                    _pictureSeriesBitmaps = value;
                    OnPropertyChanged(nameof(PictureSeriesBitmaps));
                    OnPropertyChanged(nameof(CurrentInPictureSeries));
                }
            }
        }

        private int _pictureSeriesIndex;

        private BitmapImage? _currentInPictureSeries;
        public BitmapImage? CurrentInPictureSeries
        {
            get { return _currentInPictureSeries; }
            set
            {
                if (_currentInPictureSeries != value)
                {
                    _currentInPictureSeries = value;
                    OnPropertyChanged(nameof(CurrentInPictureSeries));
                    OnPropertyChanged(nameof(IsPictureSeries));
                    OnPropertyChanged(nameof(IsPictureSeriesEnd));
                }
            }
        }

        public bool IsPictureSeries => CurrentInPictureSeries != null;
        public bool IsPictureSeriesEnd => CurrentInPictureSeries == null;

        private MediaElement _musicQuestionMediaElement;
        public MediaElement MusicQuestionMediaElement
        {
            get { return _musicQuestionMediaElement; }
            set
            {
                if (_musicQuestionMediaElement != value)
                {
                    _musicQuestionMediaElement = value;
                    OnPropertyChanged(nameof(MusicQuestionMediaElement));
                }
            }
        }

        private MediaElement _videoQuestionMediaElement;
        public MediaElement VideoQuestionMediaElement
        {
            get { return _videoQuestionMediaElement; }
            set
            {
                if (_videoQuestionMediaElement != value)
                {
                    _videoQuestionMediaElement = value;
                    OnPropertyChanged(nameof(VideoQuestionMediaElement));
                }
            }
        }

        private UserControl _currentUc;
        public UserControl CurrentUc
        {
            get { return _currentUc; }
            set
            {
                if (_currentUc != value)
                {
                    _currentUc = value;
                    OnPropertyChanged(nameof(CurrentUc));
                }
            }
        }

        SoundPlayer FinalMusicPlayer { get; set; } = new SoundPlayer();

        public RelayCommand OpenQuestionsSetupWindowCommand { get; set; }
        public RelayCommand OpenQuestionCommand { get; set; }
        public RelayCommand CloseQuestionCommand { get; set; }

        public RelayCommand ShowPictureInQustionCommand { get; set; }
        public RelayCommand NextInPictureSeriesCommand { get; set; }
        
        public RelayCommand PlayMediaInQustionCommand { get; set; }
        public RelayCommand StopMediaInQustionCommand { get; set; }

        public RelayCommand LoadMusicMediaInQustionCommand { get; set; }
        public RelayCommand LoadVideoMediaInQustionCommand { get; set; }
        public RelayCommand PlayFinalMusicCommand { get; set; }


    #endregion

        #region Players

        ObservableCollection<Player> _players = new ObservableCollection<Player>();
        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set
            {
                if (_players != value)
                {
                    _players = value;
                    OnPropertyChanged(nameof(Players));
                }
            }
        }

        int _selectedPlayerIndex = -1;
        public int SelectedPlayerIndex
        {
            get  { return _selectedPlayerIndex; }
            set
            {
                if (_selectedPlayerIndex != value)
                {
                    _selectedPlayerIndex = value;
                    OnPropertyChanged(nameof(SelectedPlayerIndex));
                    OnPropertyChanged(nameof(SelectedPlayer));
                    OnPropertyChanged(nameof(IsAvailableForScoreChange));
                }
            }
        }

        public Player SelectedPlayer =>
            SelectedPlayerIndex != -1 ? Players[SelectedPlayerIndex] : new Player("None", "#a2a2a2");

        public RelayCommand SelectPlayerCommand { get; set; }

        #endregion

        #region Statistics

        string _statisticsCsvPath = "";
        public string StatisticsCsvPath
        {
            get { return _statisticsCsvPath; }
            set
            {
                if (_statisticsCsvPath != value)
                {
                    _statisticsCsvPath = value;
                    OnPropertyChanged(nameof(ScoreBoardText));
                }
            }
        }

        bool _statisticsRecordingIsActive = false;
        public bool StatisticsRecordingIsActive
        {
            get { return _statisticsRecordingIsActive; }
            set
            {
                if (_statisticsRecordingIsActive != value)
                {
                    _statisticsRecordingIsActive = value;
                    OnPropertyChanged(nameof(StatisticsRecordingIsActive));
                }
            }
        }

        public RelayCommand StartRecordStatisticsCommand { get; set; }
        public RelayCommand RecordScoresCommand { get; set; }

        #endregion

        #region Scores

        string _scoreBoardText = "";
        public string ScoreBoardText
        {
            get { return _scoreBoardText; }
            set
            {
                if (_scoreBoardText != value)
                {
                    _scoreBoardText = value;
                    OnPropertyChanged(nameof(ScoreBoardText));
                    int score = 0;
                    if (int.TryParse(value, out score)) ScoreToChange = score;
                }
            }
        }

        int _scoreToChange = 0;
        int ScoreToChange
        {
            get { return _scoreToChange;}
            set
            {
                if (_scoreToChange != value)
                {
                    _scoreToChange = value;
                    OnPropertyChanged(nameof(ScoreToChange));
                }
            }
        }

        public bool IsAvailableForScoreChange
        {
            get
            {
                return SelectedPlayerIndex != -1
                       && (
                           (GamePhase == (int)GamePhaseEnum.Question && PlayerSelectionMode == PlayerSelectionModeEnum.Auto)
                           || PlayerSelectionMode == PlayerSelectionModeEnum.Manual
                       );
            }
        }

        bool _autoCloseuestionOnPositiveAnswer = true;
        public bool AutoCloseuestionOnPositiveAnswer
        {
            get { return _autoCloseuestionOnPositiveAnswer; }
            set
            {
                if (_autoCloseuestionOnPositiveAnswer != value)
                {
                    _autoCloseuestionOnPositiveAnswer = value;
                    OnPropertyChanged(nameof(AutoCloseuestionOnPositiveAnswer));
                }
            }
        }

        bool _autoNextPLayerOnNegativeAnswer = true;
        public bool AutoNextPLayerOnNegativeAnswer
        {
            get { return _autoNextPLayerOnNegativeAnswer; }
            set
            {
                if (_autoNextPLayerOnNegativeAnswer != value)
                {
                    _autoNextPLayerOnNegativeAnswer = value;
                    OnPropertyChanged(nameof(AutoNextPLayerOnNegativeAnswer));
                }
            }
        }

        public RelayCommand ChangePlayerScoreCommand { get; set; }

        #endregion

        #region Intro Media elements

        MediaElement _videoMediaElement = new MediaElement();
        public MediaElement VideoMediaElement
        {
            get { return _videoMediaElement; }
            set
            {
                if (_videoMediaElement != value)
                {
                    _videoMediaElement = value;
                    OnPropertyChanged(nameof(VideoMediaElement));
                }
            }
        }

        MediaElement _specialtyVideoMediaElement = new MediaElement();
        public MediaElement SpecialtyVideoMediaElement
        {
            get { return _specialtyVideoMediaElement; }
            set
            {
                if (_specialtyVideoMediaElement != value)
                {
                    _specialtyVideoMediaElement = value;
                    OnPropertyChanged(nameof(SpecialtyVideoMediaElement));
                }
            }
        }

        public RelayCommand SpecialtyVideoMediaElementLoadedCommand { get; set; }
        public RelayCommand MediaElementLoadedCommand { get; set; }
        public RelayCommand CloseSpecialIntroCommand { get; set; }

        #endregion
        
        #endregion

        public GameViewModel(IMultimediaService multimediaService)
        {
            WindowLocator = new WindowLocator();
            _multimediaService = multimediaService;

            CloseAppCommand = new RelayCommand(CloseAppMethod);
            OpenPresentScreenCommand = new RelayCommand(OpenPresentScreenMethod);
            ClosePresentScreenCommand = new RelayCommand(ClosePresentScreenMethod);

            OpenQuestionsSetupWindowCommand = new RelayCommand(OpenQuestionsSetupWindowMethod);

            ChangeGamePhaseCommand = new RelayCommand(ChangeGamePhaseMethod);
            CloseSpecialIntroCommand = new RelayCommand(CloseSpecialIntroMethod);
            
            OpenQuestionCommand = new RelayCommand(OpenQuestionMethod);
            CloseQuestionCommand = new RelayCommand(CloseQuestionMethod);
            ShowPictureInQustionCommand = new RelayCommand(ShowPictureInQustionMethod);
            NextInPictureSeriesCommand = new RelayCommand(NextInPictureSeriesMethod);
            ChangePlayerScoreCommand = new RelayCommand(ChangePlayerScoreMethod);
            SelectPlayerCommand = new RelayCommand(SelectPlayerMethod);
            PlayMediaInQustionCommand = new RelayCommand(PlayMediaInQustionMethod);
            StopMediaInQustionCommand = new RelayCommand(StopMediaInQustionMethod);
            LoadMusicMediaInQustionCommand = new RelayCommand(LoadMusicMediaInQustionMethod);
            LoadVideoMediaInQustionCommand = new RelayCommand(LoadVideoMediaInQustionMethod);
            PlayFinalMusicCommand = new RelayCommand(PlayFinalMusicMethod);
            
            MediaElementLoadedCommand = new RelayCommand(MediaElementLoadedMethod);
            SpecialtyVideoMediaElementLoadedCommand = new RelayCommand(SpecialtyVideoMediaElementLoadedMethod);

            StartRecordStatisticsCommand = new RelayCommand(StartRecordStatisticsMethod);
            RecordScoresCommand = new RelayCommand(RecordScoresMethod);
            
            GetPlayers();

            #region Buttons control

            ConnectButtonsServerCommand = new RelayCommand(ConnectButtonsServerMethod, ConnectButtonsServer_CanExecute);
            DisconnectButtonsServerCommand = new RelayCommand(DisconnectButtonsServerMethod, DisconnectButtonsServer_CanExecute);
            RequestNextPlayerCommand = new RelayCommand(RequestNextPlayerMethod);
            ResetButtonsStateCommand = new RelayCommand(ResetButtonsStateMethod);
            SetReadyForAnswersCommand = new RelayCommand(SetReadyForAnswersMethod);

            ClearWsLogCommand = new RelayCommand(ClearWsLogMethod);
            #endregion
        
        }

        #region Methods

        #region Player buttons methods

        private void SetReadyForAnswersMethod(object obj)
        {
            if (GamePhase==(int)GamePhaseEnum.Question) ReadyToCollectAnswers = true;

            try
            {
                if (WebSocketClient.Send(WsMessages.StartCommand))
                {
                    AddToLogList($"C: {WsMessages.StartCommand}");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in StartButtonsMethod, message:{e.Message}");
            }
            //_log.Info("OnStartButtonPressed");
        }

        private void ResetButtonsStateMethod(object obj)
        {
            try
            {
                if (ButtonsConnectionStatus == BtnsConnectionStatus.Connected)
                {
                    if (WebSocketClient.Send(WsMessages.ResetCommand))
                    {
                        AddToLogList($"C: {WsMessages.ResetCommand}");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ResetButtonsStateMethod, message:{e.Message}");
            }
            //_log.Info("OnResetButtonPressed");
   
        }

        private void RequestNextPlayerMethod(object obj)
        {
            try
            {
                if (WebSocketClient.Send(WsMessages.NextCommand))
                {
                    AddToLogList($"C: {WsMessages.NextCommand}");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in RequestNextPlayerMethod, message:{e.Message}");
            }
            //_log.Info("OnNextButtonPressed");

        }

        private bool ConnectButtonsServer_CanExecute(object obj)
        {
            try
            {
                return ButtonsConnectionStatus != BtnsConnectionStatus.Connected && ButtonsConnectionStatus != BtnsConnectionStatus.Connecting;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ConnectButtonsServer_CanExecute, message:{e.Message}");
                return false;
            }            
        }
        private void DisconnectButtonsServerMethod(object obj)
        {
            try
            {
                WebSocketClient.Dispose();
                AddToLogList("Connected to buttons server");

                ButtonsConnectionStatus = "Disconnected";
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in DisconnectButtonsServerMethod, message:{e.Message}");
            }


        }
        private void ConnectButtonsServerMethod(object obj)
        {
            try
            {
                WebSocketClient = new WebSocketClientProvider(ButtonServerAddress);
                WebSocketClient.Opened += Wss_Opened;
                WebSocketClient.Closed += Wss_Closed;
                WebSocketClient.Error += Wss_Error;
                WebSocketClient.Received += Wss_NewMessage;

                ButtonsConnectionStatus = BtnsConnectionStatus.Connecting;

                if (WebSocketClient.Connect())
                {
                    AddToLogList("Connected to buttons server");
                    //_log.Info("WebSocketClient Connect");

                    ButtonsConnectionStatus = BtnsConnectionStatus.Connected;
                }
                else
                {
                    AddToLogList($"Error while connection to buttons server");
                    //_log.Error("WebSocketClient Error");

                    ButtonsConnectionStatus = BtnsConnectionStatus.Error;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ConnectButtonsServerMethod, message:{e.Message}");
            }
        }
        private bool DisconnectButtonsServer_CanExecute(object obj)
        {
            try
            {
                return ButtonsConnectionStatus == BtnsConnectionStatus.Connected || ButtonsConnectionStatus == BtnsConnectionStatus.Error;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in DisconnectButtonsServer_CanExecute, message:{e.Message}");
                return false;
            }            
        }
        #endregion

        #region Game phase

        void GamePhaseUpdate()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

                switch (GamePhase)
                {
                    case (int)GamePhaseEnum.GameIntro:
                        VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/Intro.wmv", UriKind.Relative);
                        VideoMediaElement.Play();
                        break;

                    case (int)GamePhaseEnum.FirstRoundIntro:
                        VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/FirstRound.wmv", UriKind.RelativeOrAbsolute);
                        VideoMediaElement.Play();
                        break;

                    case (int)GamePhaseEnum.FirstRound:
                        CurrentRoundQuestions = AllRoundsQuestions[0];
                        if (PlayerSelectionMode == PlayerSelectionModeEnum.Auto && ButtonsConnectionStatus == BtnsConnectionStatus.Connected) ResetButtonsStateMethod(null);
                        break;

                    case (int)GamePhaseEnum.SecondRoundIntro:
                        VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/SecondRound.wmv", UriKind.RelativeOrAbsolute);
                        VideoMediaElement.Play();
                        break;

                    case (int)GamePhaseEnum.SecondRound:
                        CurrentRoundQuestions = AllRoundsQuestions[1];
                        if (PlayerSelectionMode == PlayerSelectionModeEnum.Auto && ButtonsConnectionStatus == BtnsConnectionStatus.Connected) ResetButtonsStateMethod(null);

                        break;

                    case (int)GamePhaseEnum.ThirdRoundIntro:
                        VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/ThirdRound.wmv", UriKind.RelativeOrAbsolute);
                        VideoMediaElement.Play();
                        break;

                    case (int)GamePhaseEnum.ThirdRound:
                        CurrentRoundQuestions = AllRoundsQuestions[2];
                        if (PlayerSelectionMode == PlayerSelectionModeEnum.Auto && ButtonsConnectionStatus == BtnsConnectionStatus.Connected) ResetButtonsStateMethod(null);

                        break;

                    case (int)GamePhaseEnum.FinalRoundIntro:
                        CurrentQuestion = FinalQuestion;
                        VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/FinalRound.wmv", UriKind.RelativeOrAbsolute);
                        VideoMediaElement.Play();
                        break;

                    case (int)GamePhaseEnum.FinalRound:
                        AutoCloseuestionOnPositiveAnswer = false;
                        AutoNextPLayerOnNegativeAnswer = false;
                        PlayerSelectionMode = PlayerSelectionModeEnum.Manual;                   
                        GamePhase = (int)GamePhaseEnum.Question;
                        break;

                    case (int)GamePhaseEnum.Question:
                        if (Players != null)
                        {
                            for (int i = 0; i < Players.Count; i++)
                            {
                                Players[i].isInQueue = false;
                            }
                            OnPropertyChanged(nameof(Players));
                        }

                        switch (CurrentQuestion.SpecialityType)
                        {
                            case SpecialityTypesEnum.Cat:
                                SpecialtyVideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/Cat.wmv", UriKind.RelativeOrAbsolute);
                                SpecialtyVideoMediaElement.Play();
                                break;
                            case SpecialityTypesEnum.Auction:
                                SpecialtyVideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/Auction.wmv", UriKind.RelativeOrAbsolute);
                                SpecialtyVideoMediaElement.Play();
                                break;
                            default:
                                SpecialtyVideoMediaElement.Source = null;
                                break;
                        }

                        ImageSourceQuestion = null;


                        switch (CurrentQuestion.QuestionType)
                        {
                            case QuestionTypeEnum.Text:
                                CurrentUc = new TextQuestion();
                                break;
                            case QuestionTypeEnum.Picture:
                                CurrentUc = new PictureQuestion();
                                ShowPictureInQustionMethod(MultimediaForEnum.Question);
                                break;
                            case QuestionTypeEnum.PictureSeries:
                                CurrentUc = new PictureSeriesQuestion();
                                LoadPictureSeries();
                                break;
                            case QuestionTypeEnum.Musical:
                                CurrentUc = new MusicalQuestion();
                                break;
                            case QuestionTypeEnum.Video: 
                                CurrentUc = new VideoQuestion();
                                break;
                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                if (GamePhase != (int)GamePhaseEnum.Question)
                {
                    SelectPlayerMethod(-1);
                    ReadyToCollectAnswers = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GamePhaseUpdate, message:{e.Message}");
            }
        }
        private void ChangeGamePhaseMethod(object obj)
        {
            try
            {
                GamePhase = Convert.ToInt32(obj);
                CurrentQuestion.SpecialIntroWasNotPlayed = true;
                OnPropertyChanged(nameof(CurrentQuestion));
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ChangeGamePhaseMethod, message:{e.Message}");
            }
        }

        #endregion

        #region Present screen methods

        private void LockPresentScreenMethod(bool parameter)
        {
            try
            {
                if (PlayScreenWindow != null)
                {
                    if (parameter)
                    {
                        PlayScreenWindowState = WindowState.Normal;
                        PlayScreenWindow.WindowStyle = WindowStyle.None;
                        PlayScreenWindowState = WindowState.Maximized;
                        PlayScreenWindow.Topmost = true;
                    }
                    else
                    {
                        PlayScreenWindow.Topmost = false;
                        PlayScreenWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in LockPresentScreenMethod, message:{e.Message}");
            }            
        }

        private void ClosePresentScreenMethod(object obj)
        {
            if (PlayScreenWindow != null) PlayScreenWindow.Close();
        }
        private void OpenPresentScreenMethod(object obj)
        {
            try
            {
                PlayScreenWindow = WindowLocator.PlayScreenWindow;
                PlayScreenWindow.WindowState = WindowState.Maximized;
                PlayScreenWindow.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in OpenPresentScreenMethod, message:{e.Message}");
            }
        }

        #endregion

        #region Image

        private BitmapImage GetBitmapImage(MemoryStream ms)
        {
            var bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetBitmapImage, message:{e.Message}");
            }
            return bitmap;
        }

        private BitmapImage GetBitmapImage(string path)
        {
            var bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.EndInit();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetBitmapImage, message:{e.Message}");
            }
            return bitmap;
        }

        private void LoadPictureSeries()
        {
            PictureSeriesBitmaps = new ObservableCollection<BitmapImage>();
            string multimediaId = CurrentQuestion.MediaLink;
            var mutimediaCfg = _multimediaService.GetMultimediaConfig(multimediaId);

            foreach (var fileName in mutimediaCfg.QuestionFiles)
            {
                var mutimedia = _multimediaService.GetMultimediaPath(multimediaId, MultimediaForEnum.Question, fileName);
                if (mutimedia.mediaType == MediaType.Image)
                {
                    var bitmap = GetBitmapImage(mutimedia.path);
                    PictureSeriesBitmaps.Add(bitmap);
                }

                if (PictureSeriesBitmaps.Count == 4)
                {
                    break;
                }
            }

            _pictureSeriesIndex = 0;
            CurrentInPictureSeries = PictureSeriesBitmaps[_pictureSeriesIndex];
        }

        private void ShowPictureInQustionMethod(object obj)
        {
            try
            {
                string multimediaId = CurrentQuestion.MediaLink;
                MultimediaForEnum questionFor = (MultimediaForEnum)obj;

                var mutimediaCfg = _multimediaService.GetMultimediaConfig(multimediaId);

                string fileName = string.Empty;
                if ((int)questionFor == (int)MultimediaForEnum.Question)
                {
                    fileName = mutimediaCfg.QuestionFiles.ToList()[0];
                }
                else if ((int)questionFor == (int)MultimediaForEnum.Answer)
                {
                    fileName = mutimediaCfg.AnswerFiles.ToList()[0];
                }

                var mutimedia = _multimediaService.GetMultimediaPath(multimediaId, questionFor, fileName);
                
                if (mutimedia.mediaType == MediaType.Image)
                {
                    ImageSourceQuestion = GetBitmapImage(mutimedia.path);
                }
                else
                {
                    ImageSourceQuestion = null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ShowPictureInQustionMethod, message:{e.Message}");
            }
        }

        private void NextInPictureSeriesMethod(object obj)
        {
            _pictureSeriesIndex++;
            CurrentInPictureSeries = _pictureSeriesIndex < PictureSeriesBitmaps.Count
                ? PictureSeriesBitmaps[_pictureSeriesIndex]
                : null;
        }

        #endregion

        #region Players

        private void GetPlayers()
        {
            try
            {
                var colors = DefaultPlayers.GetColors();
                var names = DefaultPlayers.GetNames();

                for (var i = 0; i < 4; i++)
                {
                    Players.Add(new Player($"Player {names[i]}", colors[i]));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetPlayers, message:{e.Message}");
            }
        }

        private void ChangePlayerScoreMethod(object obj)
        {
            try
            {
                if (SelectedPlayerIndex > -1)
                {
                    switch ((string)obj)
                    {
                        case "+":
                            Players[SelectedPlayerIndex].Score += ScoreToChange;
                            OnPropertyChanged(nameof(Players));
                            if (GamePhase == (int)GamePhaseEnum.Question && AutoCloseuestionOnPositiveAnswer)
                            {
                                CloseQuestionMethod(null);
                                //ChangeGamePhaseMethod(ActualRoundGamePhase);
                            }
                            ScoreBoardText = "0";
                            break;
                        case "-":
                            Players[SelectedPlayerIndex].Score -= ScoreToChange;
                            OnPropertyChanged(nameof(Players));
                            if (PlayerSelectionMode==PlayerSelectionModeEnum.Auto && AutoNextPLayerOnNegativeAnswer)
                            {
                                RequestNextPlayerMethod(null);
                            }

                            break;
                        default:
                            break;
                    }
                    if (PlayerSelectionMode == PlayerSelectionModeEnum.Manual)
                    {
                        SelectPlayerMethod(-1);
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ChangePlayerScoreMethod, message:{e.Message}");
            }
        }

        private void SelectPlayerMethod(object obj)
        {
            try
            {
                int index = Convert.ToInt32(obj);

                if (index != -1)
                {
                    if (Players[index].isSelected && PlayerSelectionMode == PlayerSelectionModeEnum.Manual)
                    {
                        Players[index].isSelected = false;
                    }
                    else
                    {
                        for (int i = 0; i < Players.Count; i++) Players[i].isSelected = false;
                        Players[index].isSelected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < Players.Count; i++) Players[i].isSelected = false;
                }

                OnPropertyChanged(nameof(Players));
                SelectedPlayerIndex = Players.IndexOf(Players.FirstOrDefault(x => x.isSelected));
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in SelectPlayerMethod, message:{e.Message}");
            }
        }

        void PlayerInQueueDetector(string message)
        {
            try
            {
                if (Players != null)
                {

                    char[] separator = ";".ToCharArray();
                    string[] buttonsIndexes = message.Split(separator);
                    int qIndex = Convert.ToInt32(buttonsIndexes[0]);

                    List<int> queue = new List<int>()
                    {
                        Convert.ToInt32(buttonsIndexes[1]),
                        Convert.ToInt32(buttonsIndexes[2]),
                        Convert.ToInt32(buttonsIndexes[3]),
                        Convert.ToInt32(buttonsIndexes[4])
                    };


                    if (Players != null)
                    {
                        for (int i = 0; i < Players.Count; i++)
                        {
                            Players[i].isInQueue = false;
                        }
                        OnPropertyChanged(nameof(Players));
                    }

                    for (int i = 0; i < queue.Count; i++)
                    {

                        if (queue[i] != 0)
                        {
                            switch (queue[i])
                            {
                                case (int)ButtonEnum.Red://1
                                    Players[(int)PlayerIndexEnum.Red].isInQueue = true;
                                    break;
                                case (int)ButtonEnum.Green://2
                                    Players[(int)PlayerIndexEnum.Green].isInQueue = true;
                                    break;
                                case (int)ButtonEnum.Blue://4
                                    Players[(int)PlayerIndexEnum.Blue].isInQueue = true;
                                    break;
                                case (int)ButtonEnum.Yellow://8
                                    Players[(int)PlayerIndexEnum.Yellow].isInQueue = true;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    OnPropertyChanged(nameof(Players));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in PlayerInQueueDetector, message:{e.Message}");
            }
        }

        #endregion

        #region Questions

        private void OpenQuestionsSetupWindowMethod(object obj)
        {
            try
            {
                var questionsSetupWindow = WindowLocator.QuestionsSetupWindow;
                questionsSetupWindow.ShowDialog();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in OpenQuestionsSetupWindowMethod, message:{e.Message}");
            }
        }

        private void OpenQuestionMethod(object obj)
        {
            try
            {
                Question q = (Question)obj;

                int topicIndex = -2;
                int questionIndex = -2;
                for (int i = 0; i < CurrentRoundQuestions.Count; i++)
                {
                    questionIndex = CurrentRoundQuestions[i].Questions.IndexOf(q);
                    if (questionIndex > -1)
                    {
                        topicIndex = i;
                        break;
                    }
                }
                CurrentQuestion = CurrentRoundQuestions[topicIndex].Questions[questionIndex];
                CurrentRoundQuestions[topicIndex].Questions[questionIndex].NotYetAsked = false;


                GamePhase = (int)GamePhaseEnum.Question;
                if (CurrentQuestion.SpecialityType == SpecialityTypesEnum.Cat) ScoreBoardText = CurrentQuestion.SpecialityCatPrice.ToString();
                else ScoreBoardText = CurrentQuestion.Price.ToString();

                OnPropertyChanged(nameof(CurrentRoundQuestions));
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in OpenQuestionMethod, message:{e.Message}");
            }
        }

        private void StopMediaInQustionMethod(object obj)
        {
            try
            {
                if (MusicQuestionMediaElement != null) MusicQuestionMediaElement.Stop();
                if (VideoQuestionMediaElement != null) VideoQuestionMediaElement.Stop();
                if (FinalMusicPlayer != null) FinalMusicPlayer.Stop();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in StopMediaInQustionMethod, message:{e.Message}");
            }
        }

        private void PlayMediaInQustionMethod(object obj)
        {
            try
            {
                string Id = CurrentQuestion.MediaLink;
                MultimediaForEnum questionFor = (MultimediaForEnum)obj;

                var mutimediaCfg = _multimediaService.GetMultimediaConfig(Id);

                string fileName = string.Empty;
                if ((int)questionFor == (int)MultimediaForEnum.Question)
                {
                    fileName = mutimediaCfg.QuestionFiles.ToList()[0];
                }
                else if ((int)questionFor == (int)MultimediaForEnum.Answer)
                {
                    fileName = mutimediaCfg.AnswerFiles.ToList()[0];
                }

                var whatToPlay = _multimediaService.GetMultimediaPath(Id, questionFor, fileName);

                if (CurrentQuestion.QuestionType == QuestionTypeEnum.Musical)
                {
                    MusicQuestionMediaElement.Source = new Uri(whatToPlay.path, UriKind.RelativeOrAbsolute);
                    MusicQuestionMediaElement.Play();
                }
                else if (CurrentQuestion.QuestionType == QuestionTypeEnum.Video)
                {
                    VideoQuestionMediaElement.Source = new Uri(whatToPlay.path, UriKind.RelativeOrAbsolute);
                    VideoQuestionMediaElement.Play();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in PlayMediaInQustionMethod, message:{e.Message}");
            }

        }

        private void CloseQuestionMethod(object obj)
        {
            try
            {
                if (StatisticsRecordingIsActive) RecordScoresMethod(null);
                ChangeGamePhaseMethod(ActualRoundGamePhase);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in CloseQuestionMethod, message:{e.Message}");
            }
        }


        private void PlayFinalMusicMethod(object obj)
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string finalFile = projectDirectory + @"\Resources\Sounds\FinalTime.wav";
                FinalMusicPlayer = new SoundPlayer(finalFile); //put your own .wave file path
                FinalMusicPlayer.Play();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in PlayFinalMusicMethod, message:{e.Message}");
            }
        }

        #endregion

        #region Media element

        private void SpecialtyVideoMediaElementLoadedMethod(object obj)
        {
            try
            {
                SpecialtyVideoMediaElement = (MediaElement)obj;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in SpecialtyVideoMediaElementLoadedMethod, message:{e.Message}");
            }
        }

        private void MediaElementLoadedMethod(object obj)
        {
            try
            {
                VideoMediaElement = (MediaElement)obj;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in MediaElementLoadedMethod, message:{e.Message}");
            }
        }

        private void CloseSpecialIntroMethod(object obj)
        {
            try
            {
                if (CurrentQuestion != null)
                {
                    CurrentQuestion.SpecialIntroWasNotPlayed = false;
                    OnPropertyChanged(nameof(CurrentQuestion));
                    OnPropertyChanged(nameof(CurrentRoundQuestions));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in CloseSpecialIntroMethod, message:{e.Message}");
            }
        }

        private void LoadMusicMediaInQustionMethod(object obj)
        {
            try
            {
                MusicQuestionMediaElement = (MediaElement)obj;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in LoadMusicMediaInQustionMethod, message:{e.Message}");
            }
        }
        private void LoadVideoMediaInQustionMethod(object obj)
        {
            try
            {
                VideoQuestionMediaElement = (MediaElement)obj;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in LoadVideoMediaInQustionMethod, message:{e.Message}");
            }
        }

        #endregion

        #region WS        

        private void Wss_Opened()
        {
            try
            {
                AddToLogList($"Opened");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in Wss_Opened, message:{e.Message}");
            }
        }
        private void Wss_Closed()
        {
            try
            {
                AddToLogList($"Closed");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in Wss_Closed, message:{e.Message}");
            }
        }
        private void Wss_Error(string message)
        {
            try
            {
                AddToLogList($"Error {message}");
                ButtonsConnectionStatus = BtnsConnectionStatus.Error;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in Wss_Error, message:{e.Message}");
            }
        }
        private void Wss_NewMessage(string message)
        {
            try
            {
                AddToLogList($"S: {message}");
                ButtonsMessageText = message;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in Wss_NewMessage, message:{e.Message}");
            }
        }
        private void AddToLogList(string message)
        {
            try
            {
                _dispatcher.Invoke(() => WsLogOC.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\t {message}"));
                OnPropertyChanged(nameof(WsLogSelectedIndex));
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in AddToLogList, message:{e.Message}");
            }
        }
        private void ClearWsLogMethod(object obj)
        {
            try
            {
                WsLogOC.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ClearWsLogMethod, message:{e.Message}");
            }
        }

        #endregion

        #region Statistics

        private void RecordScoresMethod(object obj)
        {
            try
            {
                if (StatisticsRecordingIsActive)
                {
                    using (StreamWriter sw = File.AppendText(StatisticsCsvPath))
                    {
                        string raw = "";
                        for (int i = 0; i < Players.Count; i++)
                        {
                            raw = raw + Players[i].Score.ToString() + ";";
                        }
                        raw = raw.Remove(raw.Length - 1);
                        sw.WriteLine(raw);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in RecordScoresMethod, message:{e.Message}");
            }
        }

        private void StartRecordStatisticsMethod(object obj)
        {
            try
            {
                StatisticsCsvPath = @"C:\SvoyaIgra\Statistics_" + DateTime.Now.ToString("HH_mm_ddMMyyyy") + ".csv";

                MessageBoxResult result = MessageBox.Show("Do you really want to start game statistics recording with these names?", "Statistics", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {

                    StatisticsRecordingIsActive = true;
                    using (StreamWriter sw = File.AppendText(StatisticsCsvPath))
                    {
                        string raw = "";
                        for (int i = 0; i < Players.Count; i++)
                        {
                            raw = raw + Players[i].Name + ";";
                        }
                        raw = raw.Remove(raw.Length - 1);
                        sw.WriteLine(raw);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in StartRecordStatisticsMethod, message:{e.Message}");
            }
        }

        #endregion

        #region Overall SW methods
        private void CloseAppMethod(object obj)
        {
            //https://preview.redd.it/wudbck9rhuw61.jpg?auto=webp&s=75ebd5191a97a9f6a4e3b2e3d5ae16c7016f57e9
            App.Current.Shutdown();
        }
        #endregion

        #endregion


    }
}
