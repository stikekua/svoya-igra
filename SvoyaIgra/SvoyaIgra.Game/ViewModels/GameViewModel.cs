using SvoyaIgra.Game.Metadata;
using SvoyaIgra.Game.ViewModels.Helpers;
using SvoyaIgra.Game.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using System.Threading;
using SvoyaIgra.Game.Views.Questions;
using System.Windows.Controls;
using System.Reflection;
using System.IO;

namespace SvoyaIgra.Game.ViewModels
{

    #region enums
    public enum GamePhaseEnum
    {
        PreGame = -1,
        GameIntro = 0,
        FirstRoundIntro = 1,
        FirstRound = 2,
        SecondRoundIntro = 3,
        SecondRound = 4,
        ThirdRoundIntro = 5,
        ThirdRound = 6,
        FinalRoundIntro = 7,
        FinalRound = 8,
        Results = 9,

        Question = 10

    }
    public enum QuestionTypeEnum
    {
        Text = 1,
        Picture = 2,
        PictureSeries = 3,
        Musical = 4,
        Video = 5
    }
    public enum PlayerColorEnum
    {
        Red = 1,
        Green = 2,
        Blue = 4,
        Yellow = 8
    }
    public enum PlayerIndexEnum
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3
    }
    public enum SpecialityTypesEnum
    {
        NotSpecial = 0,
        Cat = 1,
        Auction = 2
    }

    #endregion

    public class GameViewModel:ViewModelBase
    {
        #region Properties

        #region Cockpit window control

        public RelayCommand CloseAppCommand { get; set; }






        #endregion

        #region PlayScreen

        PlayScreenWindow _playScreenWindow=null;
        public PlayScreenWindow PlayScreenWindow 
        { 
            get
            {
                return _playScreenWindow;
            }
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

       




        public bool PlayScreenRunning
        {
            get
            {
               if (PlayScreenWindow!=null) return true;
               return false;
            }
        }

        bool _playScreenLocked = false;
        public bool PlayScreenLocked
        {
            get
            {
                return _playScreenLocked;
            }
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


        #region RelayCommands
        public RelayCommand OpenPresentScreenCommand { get; set; } 
        public RelayCommand ClosePresentScreenCommand { get; set; } 
        public RelayCommand ChangeGamePhaseCommand { get; set; }




        #endregion

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
                    OnPropertyChanged(nameof(IsAvailableForScoreChange));
                    OnPropertyChanged(nameof(isIntroOnScreen));
                    GamePhaseUpdate();
                }
            }
        }
        public bool isIntroOnScreen
        {
            get
            {
                if     ((GamePhase == (int)GamePhaseEnum.GameIntro)         ||
                        (GamePhase == (int)GamePhaseEnum.FirstRoundIntro)   ||
                        (GamePhase == (int)GamePhaseEnum.SecondRoundIntro)  ||
                        (GamePhase == (int)GamePhaseEnum.ThirdRoundIntro)   ||
                        (GamePhase == (int)GamePhaseEnum.FinalRoundIntro))      return true;
                return  false;
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

        public bool IsQuestion
        {
            get { return GamePhase == (int)GamePhaseEnum.Question ? true : false; }
        }
        #endregion

        #region Questions

        #region Setup window



        #endregion



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
                    Debug.WriteLine($"AllRoundsQuestions changed");
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

        public bool QuestionsArePrepared
        {
            get { return AllRoundsQuestions.Count > 0 ? true : false; }
        }

        public RelayCommand OpenQuestionsSetupWindowCommand { get; set; }
        public RelayCommand OpenQuestionCommand { get; set; }
        public RelayCommand CloseQuestion { get; set; }

        private Question _currentQuestion = new Question();
        public Question CurrentQuestion 
        {   get {  return _currentQuestion; }
            set 
            {
                if (_currentQuestion!=value)
                {                    
                    _currentQuestion = value;
                    OnPropertyChanged(nameof(CurrentQuestion));
                } 
            } 
        }

        private Question _finalQuestion = new Question();
        public Question FinalQuestion
        {
            get { return _finalQuestion; }
            set
            {

                Debug.WriteLine($"trying to change FinalQuestion changed, text is {FinalQuestion.QuestionText}");
                if (_finalQuestion != value)
                {
                    _finalQuestion = value;
                    OnPropertyChanged(nameof(FinalQuestion));

                    Debug.WriteLine($"FinalQuestion changed, text is {FinalQuestion.QuestionText}");
                }
            }
        }




        #endregion

        #region Players


        ObservableCollection<Player> _players = new ObservableCollection<Player>();
        public ObservableCollection<Player> Players
        {
            get
            {
                return _players;
            }
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
                    OnPropertyChanged(nameof(IsAvailableForScoreChange));
                }
            }
        }

        string _scoreBoardText = "";
        public string ScoreBoardText
        {
            get
            {
                return _scoreBoardText;
            }
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
            get
            {
                return _scoreToChange;
            }
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
                return (SelectedPlayerIndex != -1 && GamePhase == (int)GamePhaseEnum.Question) ? true:false;
            }
        }


        string _buttonsMessageText ="1;0;0;0;0";
        public string ButtonsMessageText
        {
            get { return _buttonsMessageText; }
            set
            {
                if (_buttonsMessageText != value)
                {
                    _buttonsMessageText = value;
                    OnPropertyChanged(nameof(ButtonsMessageText));

                    if (AutoPlayerSelection) SelectPlayerMethod(DecodeButtonMessage(_buttonsMessageText));
                }
            }
        }

        string _buttonsConnectionStatus = "Connected";
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

        
        public bool AutoPlayerSelection
        {
            get {  return AutoPlayerSelectionIndex==0 ? true: false; }

        }

        int _autoPlayerSelectionIndex = 0; //0 = auto, 1=manual
        public int AutoPlayerSelectionIndex
        {
            get { return _autoPlayerSelectionIndex; }
            set
            {
                if (_autoPlayerSelectionIndex != value)
                {
                    _autoPlayerSelectionIndex = value;
                    OnPropertyChanged(nameof(AutoPlayerSelectionIndex));
                    OnPropertyChanged(nameof(AutoPlayerSelection));

                    if (_autoPlayerSelectionIndex==1)
                    {
                        ButtonsMessageText = "1;0;0;0;0";
                        SelectPlayerMethod(-1);
                    }
                        
                }
            }
        }


        public RelayCommand ChangePlayerScoreCommand { get; set; }
        public RelayCommand SelectPlayerCommand { get; set; }



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

                    if (value)
                    {
                        Debug.WriteLine("Ready to collect answers");
                        ResetButtonsStateMethod(null);
                    }
                    else
                    {
                        Debug.WriteLine("Not Ready to collect answers");
                    }
                }
            }
        }


        public RelayCommand ConnectButtonsServerCommand { get; set; }
        public RelayCommand DisconnectButtonsServerCommand { get; set; }
        public RelayCommand RequestNextPlayerCommand { get; set; }
        public RelayCommand ResetButtonsStateCommand { get; set; }

        public RelayCommand SetReadyForAnswersCommand { get; set; }





        #endregion


        #endregion

        #region Media elements

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

        public GameViewModel()
        {
            CloseAppCommand  = new RelayCommand(CloseAppMethod);
            OpenPresentScreenCommand = new RelayCommand(OpenPresentScreenMethod);
            ClosePresentScreenCommand = new RelayCommand(ClosePresentScreenMethod);

            OpenQuestionsSetupWindowCommand = new RelayCommand(OpenQuestionsSetupWindowMethod);

            ChangeGamePhaseCommand = new RelayCommand(ChangeGamePhaseMethod);
            CloseSpecialIntroCommand = new RelayCommand(CloseSpecialIntroMethod);


            OpenQuestionCommand = new RelayCommand(OpenQuestionMethod);
            ChangePlayerScoreCommand = new RelayCommand(ChangePlayerScoreMethod);
            SelectPlayerCommand = new RelayCommand(SelectPlayerMethod);


            MediaElementLoadedCommand = new RelayCommand(MediaElementLoadedMethod);
            SpecialtyVideoMediaElementLoadedCommand = new RelayCommand(SpecialtyVideoMediaElementLoadedMethod);
            ///test
            //GetTestQuestionsMethod(null);
            GetPlayers();
            //OpenPresentScreenMethod(null);
            ///

            #region Buttons control

            ConnectButtonsServerCommand = new RelayCommand(ConnectButtonsServerMethod);
            DisconnectButtonsServerCommand = new RelayCommand(DisconnectButtonsServerMethod);
            RequestNextPlayerCommand = new RelayCommand(RequestNextPlayerMethod);
            ResetButtonsStateCommand = new RelayCommand(ResetButtonsStateMethod);
            SetReadyForAnswersCommand = new RelayCommand(SetReadyForAnswersMethod);

            #endregion

    }


        private int DecodeButtonMessage(string message)
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
            bool allZeros = queue.TrueForAll(x => x==0);

            if (allZeros) return -1;
            else
            {
                switch (queue[qIndex-1])
                {
                    case (int)PlayerColorEnum.Red://1
                        return (int)PlayerIndexEnum.Red;
                    case (int)PlayerColorEnum.Green://2
                        return (int)PlayerIndexEnum.Green;
                    case (int)PlayerColorEnum.Blue://4
                        return (int)PlayerIndexEnum.Blue;
                    case (int)PlayerColorEnum.Yellow://8
                        return (int)PlayerIndexEnum.Yellow;

                    default:
                        return -1;
                }
            }


        }

        private void ChangePlayerScoreMethod(object obj)
        {
            
            if (SelectedPlayerIndex>-1)
            {
                switch ((string)obj)
                {
                    case "+":
                        Players[SelectedPlayerIndex].Score += ScoreToChange;
                        if (GamePhase == (int)GamePhaseEnum.Question) ChangeGamePhaseMethod(ActualRoundGamePhase);
                        ScoreBoardText = "0";
                        break;
                    case "-":
                        Players[SelectedPlayerIndex].Score -= ScoreToChange;
                        break;
                    default:
                            break;                    
                }
                OnPropertyChanged(nameof(Players));
            }
            
        }

        private void SelectPlayerMethod(object obj)
        {
            int index = Convert.ToInt32(obj);


            if (index != -1)
            {
                if (Players[index].isSelected)
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

        #region Methods


        #region Player buttons methods

        private void SetReadyForAnswersMethod(object obj)
        {
            if (GamePhase==(int)GamePhaseEnum.Question) ReadyToCollectAnswers = true;
        }

        private void ResetButtonsStateMethod(object obj)
        {
            Debug.WriteLine("Buttons State Resetted");
        }

        private void RequestNextPlayerMethod(object obj)
        {
            Debug.WriteLine("Next Player in Queue requested");
        }

        private void DisconnectButtonsServerMethod(object obj)
        {
            Debug.WriteLine("Buttons Server Disconnected");
        }

        private void ConnectButtonsServerMethod(object obj)
        {
            Debug.WriteLine("Buttons Server Disconnected");
        }

        #endregion

        void GamePhaseUpdate()
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
                    break;

                case (int)GamePhaseEnum.SecondRoundIntro:
                    VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/SecondRound.wmv", UriKind.RelativeOrAbsolute);
                    VideoMediaElement.Play();
                    break;

                case (int)GamePhaseEnum.SecondRound:
                    CurrentRoundQuestions = AllRoundsQuestions[1];
                    break;

                case (int)GamePhaseEnum.ThirdRoundIntro:
                    VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/ThirdRound.wmv", UriKind.RelativeOrAbsolute);
                    VideoMediaElement.Play();
                    break;

                case (int)GamePhaseEnum.ThirdRound:
                    CurrentRoundQuestions = AllRoundsQuestions[2];
                    break;

                case (int)GamePhaseEnum.FinalRoundIntro:
                    VideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/FinalRound.wmv", UriKind.RelativeOrAbsolute);
                    VideoMediaElement.Play();
                    break;

                case (int)GamePhaseEnum.FinalRound:
                    CurrentQuestion = FinalQuestion;
                    Debug.WriteLine(FinalQuestion.QuestionText);
                    Debug.WriteLine(CurrentQuestion.QuestionText);
                    GamePhase = (int)GamePhaseEnum.Question;
                    break;

                case (int)GamePhaseEnum.Question:
                    if (CurrentQuestion.SpecialityType==(int)SpecialityTypesEnum.Cat)
                    {
                        SpecialtyVideoMediaElement.Source= new Uri(projectDirectory + "/Resources/Videos/Cat.wmv", UriKind.RelativeOrAbsolute);
                        SpecialtyVideoMediaElement.Play();
                    }
                    else if (CurrentQuestion.SpecialityType==(int)SpecialityTypesEnum.Auction)
                    {
                        SpecialtyVideoMediaElement.Source = new Uri(projectDirectory + "/Resources/Videos/Auction.wmv", UriKind.RelativeOrAbsolute);
                        SpecialtyVideoMediaElement.Play();
                    }
                    else
                    {
                        SpecialtyVideoMediaElement.Source = null;
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

        
        private void GetPlayers()
        {
            string[] colors = { "#FF0000", "#00FF00", "#0000FF" , "#FFFF00" };
            for (int i = 0; i < 4; i++)
            {
                Players.Add(new Player("Player " + i.ToString(), colors[i]));
            }
        }

        private void ChangeGamePhaseMethod(object obj)
        {
            GamePhase = Convert.ToInt32(obj);
            CurrentQuestion.SpecialIntroWasNotPlayed = true;
            OnPropertyChanged(nameof(CurrentQuestion));

        }

        private void LockPresentScreenMethod(bool parameter)
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

        private void ClosePresentScreenMethod(object obj)
        {
            if (PlayScreenWindow != null)
            {
                PlayScreenWindow.Close();
            }
        }

        private void OpenPresentScreenMethod(object obj)
        {
            PlayScreenWindow  = new PlayScreenWindow();
            PlayScreenWindow.DataContext = this;
            PlayScreenWindow.WindowState = WindowState.Maximized;
            PlayScreenWindow.Show();       
            
        }

        private void OpenQuestionsSetupWindowMethod(object obj)
        {
            var questionsSetupWindow = new QuestionsSetupWindow();
           // questionsSetupWindow.DataContext = new QuestionsSetupViewModel(AllRoundsQuestions,FinalQuestion);

            questionsSetupWindow.DataContext = new QuestionsSetupViewModel(this);

            bool? result = questionsSetupWindow.ShowDialog();

        }

        private void OpenQuestionMethod(object obj)
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
            if (CurrentQuestion.SpecialityType == (int)SpecialityTypesEnum.Cat) ScoreBoardText = CurrentQuestion.SpecialityCatPrice.ToString();
            else ScoreBoardText = CurrentQuestion.Price.ToString();

            
            OnPropertyChanged(nameof(CurrentRoundQuestions));
        }

        private void CloseAppMethod(object obj)
        {
            App.Current.Shutdown();
        }


        #region Media element method

        private void SpecialtyVideoMediaElementLoadedMethod(object obj)
        {
            SpecialtyVideoMediaElement = (MediaElement)obj;
        }

        private void MediaElementLoadedMethod(object obj)
        {
            VideoMediaElement = (MediaElement)obj;
        }

        private void CloseSpecialIntroMethod(object obj)
        {
            if (CurrentQuestion != null)
            {
                CurrentQuestion.SpecialIntroWasNotPlayed = false;
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(CurrentRoundQuestions));
            }
        }

        #endregion

        #endregion
    }
}
