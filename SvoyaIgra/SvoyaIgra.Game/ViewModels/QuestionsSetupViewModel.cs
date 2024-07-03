using SvoyaIgra.Dal.Services;
using SvoyaIgra.Game.Metadata;
using SvoyaIgra.Game.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SvoyaIgra.Game.Enums;
using System.Runtime.ConstrainedExecution;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.Game.ViewModels
{
    public class QuestionsSetupViewModel:ViewModelBase
    {
        private readonly ITopicService _topicService;
        private readonly IQuestionService _questionService;
        private readonly IGameService _gameService;

        public ViewModelLocator ViewModelLocator { get; set; }

        #region Properties

        private Guid _gameId = Guid.Empty;
        public Guid GameId
        {
            get => _gameId;
            set { _gameId = value; OnPropertyChanged(); }
        }

        #region All questions

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
                    OnPropertyChanged(nameof(TopicsCount));
                    ViewModelLocator.GameViewModel.AllRoundsQuestions = AllRoundsQuestions;
                }
            }
        }

        public int TopicsCount
        {
            get 
            {
                int count = 0;
                if (AllRoundsQuestions.Count>0)
                {
                    for (int i = 0; i < AllRoundsQuestions.Count; i++)
                    {
                        count = count+ AllRoundsQuestions[i].Count;
                    }
                }
                return count;
            }            
        }


        public RelayCommand GetTestQuestionsCommand { get; set; }
        public RelayCommand CreateGameCommand { get; set; }
        public RelayCommand GetLastGameCommand { get; set; }
        public RelayCommand GetRealQuestionsFromDbCommand { get; set; }

        #endregion

        #region Current question

        #region control elements

        private string _currentLanguage;
        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnPropertyChanged(nameof(CurrentLanguage));
                }
            }
        }
        public ObservableCollection<string> Languages
        {
            get
            {
                var languages = new ObservableCollection<string>(Language.SupportedLangs);
                CurrentLanguage = languages.FirstOrDefault();
                return languages;
            }
        }

        private int _currentRoundIndex = 0;
        public int CurrentRoundIndex
        {
            get { return _currentRoundIndex; }
            set
            {
                if (_currentRoundIndex != value)
                {
                    _currentRoundIndex = value;
                    OnPropertyChanged(nameof(CurrentRoundIndex));
                    RefreshQuestionMethod(null);
                }
            }
        }

        private int _currentTopicIndex = 0;
        public int CurrentTopicIndex
        {
            get { return _currentTopicIndex; }
            set
            {
                if (_currentTopicIndex != value)
                {
                    _currentTopicIndex = value;
                    OnPropertyChanged(nameof(CurrentTopicIndex));
                    RefreshQuestionMethod(null);
                }
            }
        }

        private int _currentQuestionIndex = 0;
        public int CurrentQuestionIndex
        {
            get { return _currentQuestionIndex; }
            set
            {
                if (_currentQuestionIndex != value)
                {
                    _currentQuestionIndex = value;
                    OnPropertyChanged(nameof(CurrentQuestionIndex));
                    RefreshQuestionMethod(null);
                }
            }
        }

        #endregion

        private Question _currentQuestion = new Question();
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                if (_currentQuestion != value)
                {
                    _currentQuestion = value;
                    OnPropertyChanged(nameof(CurrentQuestion));
                }
            }
        }

        #endregion

        #region New question

        private string _newQuestionText = "Some Text";
        public string NewQuestionText
        {
            get { return _newQuestionText; }
            set
            {
                if (_newQuestionText != value)
                {
                    _newQuestionText = value;
                    OnPropertyChanged(nameof(NewQuestionText));
                }
            }
        }

        private string _newQuestionAnswer = "Some answer";
        public string NewQuestionAnswer
        {
            get { return _newQuestionAnswer; }
            set
            {
                if (_newQuestionAnswer != value)
                {
                    _newQuestionAnswer = value;
                    OnPropertyChanged(nameof(NewQuestionAnswer));
                }
            }
        }

        private string _newQuestionMediaLink = "";
        public string NewQuestionMediaLink
        {
            get { return _newQuestionMediaLink; }
            set
            {
                if (_newQuestionMediaLink != value)
                {
                    _newQuestionMediaLink = value;
                    OnPropertyChanged(nameof(NewQuestionMediaLink));
                }
            }
        }

        private string _newTopicName = "some new topic name";
        public string NewTopicName
        {
            get { return _newTopicName; }
            set
            {
                if (_newTopicName != value)
                {
                    _newTopicName = value;
                    OnPropertyChanged(nameof(NewTopicName));
                }
            }
        }

        private string _newQuestionSpecialityCatPrice = "100";
        public string NewQuestionSpecialityCatPrice
        {
            get { return _newQuestionSpecialityCatPrice; }
            set
            {
                if (_newQuestionSpecialityCatPrice != value)
                {
                    _newQuestionSpecialityCatPrice = value;
                    OnPropertyChanged(nameof(NewQuestionSpecialityCatPrice));
                }
            }
        }

        private QuestionTypeEnum _newQuestionType;
        public QuestionTypeEnum NewQuestionType
        {
            get { return _newQuestionType; }
            set
            {
                if (_newQuestionType != value)
                {
                    _newQuestionType = value;
                    OnPropertyChanged(nameof(NewQuestionType));
                    if (NewQuestionType==QuestionTypeEnum.Text)
                    {
                        NewQuestionMediaLink = "00000000-0000-0000-0000-000000000000";
                    }
                }
            }
        }

        private SpecialityTypesEnum _newQuestionSpecialityType;
        public SpecialityTypesEnum NewQuestionSpecialityType
        {
            get { return _newQuestionSpecialityType; }
            set
            {
                if (_newQuestionSpecialityType != value)
                {
                    _newQuestionSpecialityType = value;
                    OnPropertyChanged(nameof(NewQuestionSpecialityType));
                }
            }
        }

        public RelayCommand GetRandomCatQuestionsCommand { get; set; }
        public RelayCommand ApplyQuestionChangesCommand { get; set; }
        public RelayCommand ClearNewQuestionFieldsCommand { get; set; }
        public RelayCommand RefreshQuestionCommand{ get; set; }


    #endregion

        #region Final question

        private Question _finalQuestionSetup = new Question();
        public Question FinalQuestionSetup
        {
            get { return _finalQuestionSetup; }
            set
            {
                if (_finalQuestionSetup != value)
                {
                    _finalQuestionSetup = value;
                    OnPropertyChanged(nameof(FinalQuestionSetup));
                    ViewModelLocator.GameViewModel.FinalQuestion = FinalQuestionSetup;
                }
            }
        }


        private string _newFinalQuestionText = "Some Final Text";
        public string NewFinalQuestionText
        {
            get { return _newFinalQuestionText; }
            set
            {
                if (_newFinalQuestionText != value)
                {
                    _newFinalQuestionText = value;
                    OnPropertyChanged(nameof(NewFinalQuestionText));
                }
            }
        }


        private string _newFinalQuestionAnswer = "Some final answer";
        public string NewFinalQuestionAnswer
        {
            get { return _newFinalQuestionAnswer; }
            set
            {
                if (_newFinalQuestionAnswer != value)
                {
                    _newFinalQuestionAnswer = value;
                    OnPropertyChanged(nameof(NewFinalQuestionAnswer));
                }
            }
        }


        private string _newFinalQuestionTopicName = "Some final topic";
        public string NewFinalQuestionTopicName
        {
            get { return _newFinalQuestionTopicName; }
            set
            {
                if (_newFinalQuestionTopicName != value)
                {
                    _newFinalQuestionTopicName = value;
                    OnPropertyChanged(nameof(NewFinalQuestionTopicName));
                }
            }
        }

        public List<Question> FinalQuestions { get; set; }
        public int SelectedFinalQuestionIndex { get; set; } = 0;

        public RelayCommand ApplyFinalQuestionChangesCommand { get; set; }
        public RelayCommand GetNewFinalQuestionCommand { get; set; }

        #endregion

        #endregion

        public QuestionsSetupViewModel(ITopicService topicService, IQuestionService questionService, IGameService gameService)
        {
            ViewModelLocator = new ViewModelLocator();

            _topicService = topicService;
            _questionService = questionService;
            _gameService = gameService;

            ApplyQuestionChangesCommand = new RelayCommand(ApplyQuestionChangesMethod);
            GetTestQuestionsCommand = new RelayCommand(GetTestQuestionsMethod);
            CreateGameCommand = new RelayCommand(CreateGame_Execute, CreateGame_CanExecute);
            GetLastGameCommand = new RelayCommand(GetLastGame_Execute);
            GetRealQuestionsFromDbCommand = new RelayCommand(GetRealQuestionsFromDb_Execute, GetRealQuestionsFromDb_CanExecute);
            GetRandomCatQuestionsCommand = new RelayCommand(GetRandomCatQuestionsMethod);
            ApplyFinalQuestionChangesCommand = new RelayCommand(ApplyFinalQuestionChangesMethod);
            GetNewFinalQuestionCommand = new RelayCommand(GetNewFinalQuestionMethod);
            ClearNewQuestionFieldsCommand = new RelayCommand(ClearNewQuestionFieldsMethod);
            RefreshQuestionCommand = new RelayCommand(RefreshQuestionMethod);
        }

        #region Methods

        #region All questions

        private void GetTestQuestionsMethod(object obj)
        {
            try
            {
                var rounds = new ObservableCollection<ObservableCollection<Topic>>();

                for (int z = 0; z < 3; z++)//rounds
                {
                    var topics = new ObservableCollection<Topic>();
                    for (int i = 0; i < 6; i++) //topics
                    {
                        var listOfQuestions = new List<Question>();
                        string topic = "Topic " + i.ToString();
                        for (int k = 0; k < 5; k++) //questions
                        {

                            string questionText = topic + " question " + k.ToString();
                            string answer = "answer for " + questionText;
                            string mediaLink = "00000000-0000-0000-0000-000000000000";
                            if ((QuestionTypeEnum)k + 1 == QuestionTypeEnum.Musical)
                            {
                                mediaLink = "00000000-0000-0000-0000-000000000001"; //default music
                            }
                            else if ((QuestionTypeEnum)k + 1 == QuestionTypeEnum.Video)
                            {
                                mediaLink = "00000000-0000-0000-0000-000000000002"; //default video
                            }
                            listOfQuestions.Add(new Question(topic, questionText, answer, (k * 100 + 100) * (z + 1), (QuestionTypeEnum)k + 1, mediaLink));
                        }
                        topics.Add(new Topic(listOfQuestions, "Round " + (z + 1).ToString() + " Topic " + i.ToString()));
                    }
                    rounds.Add(topics);
                }

                AllRoundsQuestions = rounds;
                FinalQuestionSetup = new Question("some final question text", "some final question answer", 0, (QuestionTypeEnum)1, true, "Just Final question");
                RefreshQuestionMethod(null);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetTestQuestionsMethod, message: {e.Message}");
            }            
        }

        private bool CreateGame_CanExecute(object obj)
        {
            return true;
            //TODO prevent multiple game creation
        }

        private async void CreateGame_Execute(object obj)
        {
            try
            {
                GameId = await _gameService.CreateGameAsync(CurrentLanguage);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in CreateGame_Execute, message: {e.Message}");
            }
        }

        private async void GetLastGame_Execute(object obj)
        {
            try
            {
                GameId = await _gameService.GetLastGameAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetLastGame_Execute, message: {e.Message}");
            }
        }

        private bool GetRealQuestionsFromDb_CanExecute(object obj)
        {
            try
            {
                return GameId != Guid.Empty;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetRealQuestionsFromDb_CanExecute, message: {e.Message}");
                return false;
            }
            
        }

        private async void GetRealQuestionsFromDb_Execute(object obj)
        {
            try
            {

                //get round topics
                var topicsFromDB = await _gameService.GetTopicsAsync(GameId);

                var rounds = new ObservableCollection<ObservableCollection<Topic>>();

                var topics = new ObservableCollection<Topic>();

                topicsFromDB.ToList().ForEach(t =>
                {
                    var listOfQuestions = new List<Question>();
                    listOfQuestions.AddRange(
                        t.Questions!.Select(q => new Question(t.Name, q.Text, q.Answer, (int)q.Difficulty, (QuestionTypeEnum)q.Type, q.MultimediaId ?? Guid.Empty.ToString())));

                    topics.Add(new Topic(listOfQuestions, t.Name));
                });

                for (int i = 0; i < topics.Count; i++)
                {
                    if (i <= 5) //first round
                    {
                        for (int k = 0; k < topics[i].Questions.Count; k++)
                        {
                            topics[i].Questions[k].Price = topics[i].Questions[k].Price * 100;
                        }
                    }
                    else if (i >= 6 && i <= 11) //second round
                    {
                        for (int k = 0; k < topics[i].Questions.Count; k++)
                        {
                            topics[i].Questions[k].Price = topics[i].Questions[k].Price * 200;
                        }
                    }
                    else if (i >= 12) //third round
                    {
                        for (int k = 0; k < topics[i].Questions.Count; k++)
                        {
                            topics[i].Questions[k].Price = topics[i].Questions[k].Price * 300;
                        }
                    }
                }

                rounds.Add(new ObservableCollection<Topic>(topics.Take(6)));
                rounds.Add(new ObservableCollection<Topic>(topics.Skip(6).Take(6)));
                rounds.Add(new ObservableCollection<Topic>(topics.Skip(12).Take(6)));

                AllRoundsQuestions = rounds;

                //get final topics
                var finaltopicsFromDB = await _gameService.GetTopicsFinalAsync(GameId);
                //TODO 
                FinalQuestions = new List<Question>();
                finaltopicsFromDB.ToList().ForEach(q =>
                {
                    var question = q.Questions!.ToList()[0];
                    FinalQuestions.Add(new Question(question.Text, question.Answer, 0, QuestionTypeEnum.Text, true, q.Name));
                });
                //temp
                FinalQuestionSetup = FinalQuestions[0];
                //end temp
                RefreshQuestionMethod(null);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetRealQuestionsFromDb_Execute, message: {e.Message}");
            }

        }
        void GetCurrentQuestion()
        {
            try
            {
                if (AllRoundsQuestions.Count > 0)
                {
                    CurrentQuestion = AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex];
                    OnPropertyChanged(nameof(CurrentQuestion));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetCurrentQuestion, message: {e.Message}");
            }
        }

        #endregion

        #region Round qustions change

        private async void GetRandomCatQuestionsMethod(object obj)
        {
            try
            {
                var catTopicFromDB = await _gameService.GetCatQuestionAsync(GameId);
                var questionItself = catTopicFromDB.Questions.ToList()[0];
                var q = questionItself.Type;

                Question questionFromDb = new Question(questionItself.Text, questionItself.Answer, 0, (QuestionTypeEnum)questionItself.Type, true, catTopicFromDB.Name);
                questionFromDb.MediaLink = questionItself.MultimediaId;

                NewQuestionText = questionFromDb.QuestionText;
                NewQuestionAnswer = questionFromDb.QuestionAnswer;
                NewQuestionSpecialityType = SpecialityTypesEnum.Cat;
                NewTopicName = questionFromDb.TopicName;
                NewQuestionType = (QuestionTypeEnum)questionFromDb.QuestionType;
                NewQuestionMediaLink = questionFromDb.MediaLink;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in GetRandomCatQuestionsMethod, message: {e.Message}");
            }
        }
        private void ApplyQuestionChangesMethod(object obj)
        {
            try
            {
                int newCatPrice = 0;
                if (!NewQuestionAnswer.Equals(string.Empty) && !NewQuestionText.Equals(string.Empty) && Int32.TryParse(NewQuestionSpecialityCatPrice, out newCatPrice))
                {
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionText = NewQuestionText;
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionAnswer = NewQuestionAnswer;
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityType = NewQuestionSpecialityType;
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionType = NewQuestionType;

                    if (NewQuestionType != QuestionTypeEnum.Text)
                    {
                        AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].MediaLink = NewQuestionMediaLink;
                    }
                    if (NewQuestionSpecialityType == SpecialityTypesEnum.Cat)
                    {
                        AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityCatPrice = newCatPrice;
                        AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].TopicName = NewTopicName;
                    }
                }
                OnPropertyChanged(nameof(AllRoundsQuestions));

                MessageBox.Show($"In round {CurrentRoundIndex + 1} in topic {AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Name} Question number {CurrentQuestionIndex + 1} was changed, new question type is {AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionType}");
                RefreshQuestionMethod(null);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ApplyQuestionChangesMethod, message: {e.Message}");
            }
        }

        private void ClearNewQuestionFieldsMethod(object obj)
        {
            try
            {
                NewQuestionText = "";
                NewQuestionAnswer = "";
                NewQuestionSpecialityType = SpecialityTypesEnum.NotSpecial;
                NewTopicName = "";
                NewQuestionType = QuestionTypeEnum.Text;
                NewQuestionMediaLink = "00000000-0000-0000-0000-000000000000";
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ClearNewQuestionFieldsMethod, message: {e.Message}");
            }
        }

        private void RefreshQuestionMethod(object obj)
        {
            try
            {
                GetCurrentQuestion();
                if (CurrentQuestion != null)
                {
                    NewQuestionText = CurrentQuestion.QuestionText;
                    NewQuestionType = CurrentQuestion.QuestionType;
                    NewQuestionAnswer = CurrentQuestion.QuestionAnswer;
                    NewQuestionSpecialityType = CurrentQuestion.SpecialityType;
                    NewTopicName = CurrentQuestion.TopicName;
                    NewQuestionMediaLink = CurrentQuestion.MediaLink;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in RefreshQuestionMethod, message: {e.Message}");
            }
        }

        #endregion

        #region Final question
        private void ApplyFinalQuestionChangesMethod(object obj)
        {
            try
            {
                FinalQuestionSetup = new Question(NewFinalQuestionText, NewFinalQuestionAnswer, 0, QuestionTypeEnum.Text, true, NewFinalQuestionTopicName);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ApplyFinalQuestionChangesMethod, message: {e.Message}");
            }
        }

        //ToDo
        private void GetNewFinalQuestionMethod(object obj)
        {            
            try
            {
                if (FinalQuestions.Count > 0)
                {
                    if (SelectedFinalQuestionIndex != FinalQuestions.Count - 1) SelectedFinalQuestionIndex++;
                    else SelectedFinalQuestionIndex = 0;

                    FinalQuestionSetup = FinalQuestions[SelectedFinalQuestionIndex];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some problem in ApplyFinalQuestionChangesMethod, message: {e.Message}");
            }
        }
        #endregion

        #endregion
        
    }
}
