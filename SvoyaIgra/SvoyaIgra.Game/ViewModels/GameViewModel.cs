using SvoyaIgra.Game.Metadata;
using SvoyaIgra.Game.ViewModels.Helpers;
using SvoyaIgra.Game.Views;
using SvoyaIgra.Game.Views.GamePhases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Documents;

namespace SvoyaIgra.Game.ViewModels
{
    public enum GamePhaseEnum
    {
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

        #region Game phase

        private int _gamePhase = 0;
        public int GamePhase
        {
            get { return _gamePhase; }
            set
            {
                PreviousGamePhase = _gamePhase;
                if (_gamePhase != value)
                {
                    _gamePhase = value;
                    OnPropertyChanged(nameof(GamePhase));
                    GamePhaseUpdate();
                }
            }
        }

        private int _previousGamePhase = 0;
        public int PreviousGamePhase
        {
            get { return _previousGamePhase; }
            set
            {
                if (_previousGamePhase != value)
                {
                    _previousGamePhase = value;
                    OnPropertyChanged(nameof(PreviousGamePhase));
                }
            }
        }


        public bool IsGameIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.GameIntro ? true : false; }
        }
        public bool IsFirstRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.FirstRoundIntro ? true : false; }
        }
        public bool IsFirstRound
        {
            get { return GamePhase == (int)GamePhaseEnum.FirstRound ? true : false; }
        }
        public bool IsSecondRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.SecondRoundIntro ? true : false; }
        }
        public bool IsSecondRound
        {
            get { return GamePhase == (int)GamePhaseEnum.SecondRound ? true : false; }
        }
        public bool IsThirdRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.ThirdRoundIntro ? true : false; }
        }
        public bool IsThirdRound
        {
            get { return GamePhase == (int)GamePhaseEnum.ThirdRound ? true : false; }
        }
        public bool IsFinalRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.FinalRoundIntro ? true : false; }        }
        public bool IsFinalRound
        {
            get { return GamePhase == (int)GamePhaseEnum.FinalRound ? true : false; }
        }
        public bool IsQuestion
        {
            get { return GamePhase == (int)GamePhaseEnum.Question ? true : false; }
        }
        #endregion




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


        #region Questions

        private List<RoundQuestions> AllRoundsQuestions { get; set; } = new List<RoundQuestions>();

        private RoundQuestions _currentRoundQuestions;
        public RoundQuestions CurrentRoundQuestions
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

        

        public RelayCommand GetQuestions { get; set; }
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

        #endregion


        #endregion

        public GameViewModel()
        {
            CloseAppCommand  = new RelayCommand(CloseAppMethod);
            OpenPresentScreenCommand = new RelayCommand(OpenPresentScreenMethod);
            ClosePresentScreenCommand = new RelayCommand(ClosePresentScreenMethod);

            ChangeGamePhaseCommand = new RelayCommand(ChangeGamePhaseMethod);

            GetQuestions = new RelayCommand(GetQuestionsMethod);
            OpenQuestionCommand = new RelayCommand(OpenQuestionMethod);

            ///test
            GetQuestionsMethod(null);
            OpenPresentScreenMethod(null);
            ///

        }





        #region Methods

        void GamePhaseUpdate()
        {

            if (GamePhase == (int)GamePhaseEnum.FirstRound) CurrentRoundQuestions = AllRoundsQuestions[0];
            else if (GamePhase == (int)GamePhaseEnum.SecondRound) CurrentRoundQuestions = AllRoundsQuestions[1];
            else if (GamePhase == (int)GamePhaseEnum.ThirdRound) CurrentRoundQuestions = AllRoundsQuestions[2];


            OnPropertyChanged(nameof(IsGameIntro));
            OnPropertyChanged(nameof(IsFirstRoundIntro));
            OnPropertyChanged(nameof(IsFirstRound));
            OnPropertyChanged(nameof(IsSecondRoundIntro));
            OnPropertyChanged(nameof(IsSecondRound));
            OnPropertyChanged(nameof(IsThirdRoundIntro));
            OnPropertyChanged(nameof(IsThirdRound));
            OnPropertyChanged(nameof(IsFinalRoundIntro));
            OnPropertyChanged(nameof(IsFinalRound));
            OnPropertyChanged(nameof(IsQuestion));
        }

        private void GetQuestionsMethod(object obj)
        {
            for (int z = 0; z < 3; z++)//rounds
            {
                var topics = new List<Topic>();
                for (int i = 0; i < 6; i++) //topics
                {
                    var listOfQuestions = new List<Question>();
                    for (int k = 0; k < 5; k++) //questions
                    {
                        listOfQuestions.Add(new Question("question " + k.ToString(), (k * 100 + 100) * (z + 1),k+1));
                    }
                    topics.Add(new Topic(listOfQuestions, "Round " + (z + 1).ToString() + " Topic " + i.ToString()));

                }
                AllRoundsQuestions.Add(new RoundQuestions(topics));
            }
            //FirstRoundDataContext = new RoundDataContext(1, new RoundQuestions(topics),IsFirstRound);
        }

        private void ChangeGamePhaseMethod(object obj)
        {
            ChangeGamePhaseMethod(Convert.ToInt32(obj));
        }

        private void ChangeGamePhaseMethod(int phaseNumber)
        {
            if (PlayScreenWindow != null) GamePhase = phaseNumber;
        }

        private void LockPresentScreenMethod(bool parameter)
        {
            if (PlayScreenWindow != null)
            {
                if (parameter)
                {
                    PlayScreenWindowState = WindowState.Maximized;
                    PlayScreenWindow.Topmost = true;
                    PlayScreenWindow.WindowStyle = WindowStyle.None;
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

        private void OpenQuestionMethod(object obj)
        {
            var values = (object[])obj;
            int topicIndex = (int)values[0];
            int questionIndex = (int)values[1];

            CurrentQuestion = CurrentRoundQuestions.Topics[topicIndex].Questions[questionIndex];

            GamePhase = (int)GamePhaseEnum.Question;

            CurrentRoundQuestions.Topics[topicIndex].Questions[questionIndex].NotYetAsked = false;

            OnPropertyChanged(nameof(CurrentRoundQuestions));
        }



        private void CloseAppMethod(object obj)
        {
            App.Current.Shutdown();
        }

        #endregion
    }
}
