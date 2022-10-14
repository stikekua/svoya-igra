using SvoyaIgra.Game.Metadata;
using SvoyaIgra.Game.ViewModels.Helpers;
using SvoyaIgra.Game.Views;
using SvoyaIgra.Game.Views.GamePhases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Documents;
using System.Linq;

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
        public RelayCommand OpenQuestionCommandExtended { get; set; }
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
        int SelectedPlayerIndex
        {
            get
            {
                return _selectedPlayerIndex;
            }
            set
            {
                if (_selectedPlayerIndex != value)
                {
                    _selectedPlayerIndex = value;
                    OnPropertyChanged(nameof(SelectedPlayerIndex));
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




        public RelayCommand ChangePlayerScoreCommand { get; set; }
        public RelayCommand SelectPlayerCommand { get; set; }

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
            OpenQuestionCommandExtended = new RelayCommand(OpenQuestionMethodExtended);
            ChangePlayerScoreCommand = new RelayCommand(ChangePlayerScoreMethod);
            SelectPlayerCommand = new RelayCommand(SelectPlayerMethod);

            ///test
            GetQuestionsMethod(null);
            GetPlayers();
            //OpenPresentScreenMethod(null);
            ///

        }

        private void OpenQuestionMethodExtended(object obj)
        {
            Question q = (Question)obj;
            int topicIndex = -2;
            int questionIndex = -2;
            for (int i = 0; i < CurrentRoundQuestions.Topics.Count; i++)
            {
                topicIndex = CurrentRoundQuestions.Topics[i].Questions.IndexOf(CurrentRoundQuestions.Topics[i].Questions.FirstOrDefault(x=>x==q));
                if (topicIndex > -1) break;                
            }
            questionIndex = CurrentRoundQuestions.Topics[topicIndex].Questions.IndexOf(q);



            CurrentQuestion = CurrentRoundQuestions.Topics[topicIndex].Questions[questionIndex];

            GamePhase = (int)GamePhaseEnum.Question;

            CurrentRoundQuestions.Topics[topicIndex].Questions[questionIndex].NotYetAsked = false;

            OnPropertyChanged(nameof(CurrentRoundQuestions));

            ScoreBoardText = CurrentQuestion.Price.ToString();
        }

        private void ChangePlayerScoreMethod(object obj)
        {
            
            if (SelectedPlayerIndex>-1)
            {
                switch ((string)obj)
                {
                    case "+":
                        Players[SelectedPlayerIndex].Score += ScoreToChange;
                        if (GamePhase==(int)GamePhaseEnum.Question) ChangeGamePhaseMethod(PreviousGamePhase);    
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
            for (int i = 0; i < Players.Count; i++)
            {
                if (i!=index) Players[i].isSelected = false;
            }

            OnPropertyChanged(nameof(Players));
            SelectedPlayerIndex = Players.IndexOf(Players.FirstOrDefault(x => x.isSelected));
        }

        #region Methods

        void GamePhaseUpdate()
        { 
            if      (GamePhase == (int)GamePhaseEnum.FirstRound)    CurrentRoundQuestions = AllRoundsQuestions[0];
            else if (GamePhase == (int)GamePhaseEnum.SecondRound)   CurrentRoundQuestions = AllRoundsQuestions[1];
            else if (GamePhase == (int)GamePhaseEnum.ThirdRound)    CurrentRoundQuestions = AllRoundsQuestions[2];

            if (GamePhase != (int)GamePhaseEnum.Question) SelectPlayerMethod(-1);
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
        private void GetPlayers()
        {
            string[] colors = { "#FF0000", "#00FF00", "#FFFF00", "#0000FF" };
            for (int i = 0; i < 4; i++)
            {
                Players.Add(new Player("Player " + i.ToString(), colors[i]));
            }
        }

        private void ChangeGamePhaseMethod(object obj)
        {
            GamePhase = Convert.ToInt32(obj);
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
            Question q = (Question)obj;
            Debug.WriteLine(q.QuestionText + " " + q.Price.ToString());

            int topicIndex = -2;
            int questionIndex = -2;
            for (int i = 0; i < CurrentRoundQuestions.Topics.Count; i++)
            {
                questionIndex = CurrentRoundQuestions.Topics[i].Questions.IndexOf(q);
                if (questionIndex > -1)
                {
                    topicIndex = i;
                    break;
                }
                    
            }
            //Debug.WriteLine("topicIndex " + topicIndex);
            //questionIndex = CurrentRoundQuestions.Topics[topicIndex].Questions.IndexOf(q);
            //Debug.WriteLine("questionIndex " + questionIndex);
            CurrentQuestion = CurrentRoundQuestions.Topics[topicIndex].Questions[questionIndex];
            CurrentRoundQuestions.Topics[topicIndex].Questions[questionIndex].NotYetAsked = false;


            GamePhase = (int)GamePhaseEnum.Question;
            ScoreBoardText = CurrentQuestion.Price.ToString();
            OnPropertyChanged(nameof(CurrentRoundQuestions));
        }



        private void CloseAppMethod(object obj)
        {
            App.Current.Shutdown();
        }

        #endregion
    }
}
