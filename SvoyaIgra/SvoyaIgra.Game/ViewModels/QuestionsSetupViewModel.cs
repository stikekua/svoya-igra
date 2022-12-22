using SvoyaIgra.Dal.Services;
using SvoyaIgra.Game.Metadata;
using SvoyaIgra.Game.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using SvoyaIgra.Game.Enums;

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

        private int _newQuestionTypeIndex;
        public int NewQuestionTypeIndex
        {
            get { return _newQuestionTypeIndex; }
            set
            {
                if (_newQuestionTypeIndex != value)
                {
                    _newQuestionTypeIndex = value;
                    OnPropertyChanged(nameof(NewQuestionTypeIndex));
                }
            }
        }

        private int _newQuestionSpecialityTypeIndex;
        public int NewQuestionSpecialityTypeIndex
        {
            get { return _newQuestionSpecialityTypeIndex; }
            set
            {
                if (_newQuestionSpecialityTypeIndex != value)
                {
                    _newQuestionSpecialityTypeIndex = value;
                    OnPropertyChanged(nameof(NewQuestionSpecialityTypeIndex));
                    OnPropertyChanged(nameof(CurrentQuestion));
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
                        if (k+1==(int)QuestionTypeEnum.Musical)
                        {
                            mediaLink = "00000000-0000-0000-0000-000000000001"; //default music
                        }
                        else if (k+1 == (int)QuestionTypeEnum.Video)
                        {
                            mediaLink = "00000000-0000-0000-0000-000000000002"; //default video
                        } 
                        //listOfQuestions.Add(new Question(questionText, "Answer for " + questionText, (k * 100 + 100) * (z + 1), k + 1,true, "Topic " + i.ToString()));
                        listOfQuestions.Add(new Question(topic, questionText, answer, (k * 100 + 100) * (z + 1), k + 1,mediaLink ));
                    }
                    topics.Add(new Topic(listOfQuestions, "Round " + (z + 1).ToString() + " Topic " + i.ToString()));

                }

                rounds.Add(topics);                
            }

            AllRoundsQuestions = rounds;

            FinalQuestionSetup = new Question("some final question text", "some final question answer", 0,1,true,"Just Final question");


            RefreshQuestionMethod(null);
        }

        private bool CreateGame_CanExecute(object obj)
        {
            return true;
            //TODO prevent multiple game creation
        }

        private async void CreateGame_Execute(object obj)
        {
            GameId = await _gameService.CreateGameAsync();
        }

        private async void GetLastGame_Execute(object obj)
        {
            GameId = await _gameService.GetLastGameAsync();
        }

        private bool GetRealQuestionsFromDb_CanExecute(object obj)
        {
            return GameId != Guid.Empty;
        }

        private async void GetRealQuestionsFromDb_Execute(object obj)
        {
            //get round topics
            var topicsFromDB = await _gameService.GetTopicsAsync(GameId);

            var rounds = new ObservableCollection<ObservableCollection<Topic>>();

            var topics = new ObservableCollection<Topic>();

            topicsFromDB.ToList().ForEach(t =>
            {
                var listOfQuestions = new List<Question>();
                listOfQuestions.AddRange(
                    t.Questions!.Select(q => new Question(t.Name, q.Text, q.Answer, (int)q.Difficulty, (int)q.Type, q.MultimediaId ?? Guid.Empty.ToString())));

                topics.Add(new Topic(listOfQuestions, t.Name));                
            });

            for (int i = 0; i < topics.Count; i++)
            {
                if (i <= 5)
                {
                    for (int k = 0; k < topics[i].Questions.Count; k++)
                    {
                        topics[i].Questions[k].Price = topics[i].Questions[k].Price * 100;
                    }                    
                }
                else if (i>=6 && i<=11)
                {
                    for (int k = 0; k < topics[i].Questions.Count; k++)
                    {
                        topics[i].Questions[k].Price = topics[i].Questions[k].Price * 200;
                    }
                }
                else if (i >= 12)
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
                FinalQuestions.Add(new Question(question.Text, question.Answer, 0, 1, true, q.Name));
            }); 


            //temp
            FinalQuestionSetup = FinalQuestions[0];
            //end temp

            RefreshQuestionMethod(null);

        }
        void GetCurrentQuestion()
        {
            if (AllRoundsQuestions.Count > 0)
            {
                CurrentQuestion = AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex];
            }
        }

        #endregion

        #region Round qustions change

        private async void GetRandomCatQuestionsMethod(object obj)
        {

            var catTopicFromDB = await _gameService.GetCatQuestionAsync(GameId);
            var questionItself = catTopicFromDB.Questions.ToList()[0];
            var q = questionItself.Type;
            

            Question questionFromDb = new Question(questionItself.Text, questionItself.Answer, 0, (int)questionItself.Type,true, catTopicFromDB.Name );
            questionFromDb.MediaLink = questionItself.MultimediaId;

            NewQuestionText = questionFromDb.QuestionText;
            NewQuestionAnswer = questionFromDb.QuestionAnswer;
            //NewQuestionSpecialityTypeIndex = 1;//Cat
            NewQuestionSpecialityType = SpecialityTypesEnum.Cat;//Cat
            NewTopicName = questionFromDb.TopicName;
            NewQuestionTypeIndex = questionFromDb.QuestionType - 1;
            NewQuestionMediaLink = questionFromDb.MediaLink;
        }
        private void ApplyQuestionChangesMethod(object obj)
        {
            int newCatPrice = 0;
            if (!NewQuestionAnswer.Equals(string.Empty) && !NewQuestionText.Equals(string.Empty) && Int32.TryParse(NewQuestionSpecialityCatPrice, out newCatPrice))
            {
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionText = NewQuestionText;
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionAnswer = NewQuestionAnswer;
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityType = NewQuestionSpecialityType;// here
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionType = NewQuestionTypeIndex+1;

                if (NewQuestionTypeIndex + 1 > (int)QuestionTypeEnum.Text) //if not text
                {
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].MediaLink = NewQuestionMediaLink;
                }
                //if (NewQuestionSpecialityTypeIndex == 1)
                //{
                //    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityCatPrice = newCatPrice;
                //    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].TopicName = NewTopicName;
                //}
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

        private void ClearNewQuestionFieldsMethod(object obj)
        {
            NewQuestionText = "";
            NewQuestionAnswer = "";
            NewQuestionSpecialityTypeIndex = 0;
            NewTopicName = "";
            NewQuestionTypeIndex = 0;
            NewQuestionMediaLink = "00000000-0000-0000-0000-000000000000";
        }

        private void RefreshQuestionMethod(object obj)
        {
            GetCurrentQuestion();
            if (CurrentQuestion!=null)
            {
                NewQuestionText = CurrentQuestion.QuestionText;
                NewQuestionAnswer = CurrentQuestion.QuestionAnswer;
                // NewQuestionSpecialityTypeIndex = CurrentQuestion.SpecialityType-1;
                NewQuestionSpecialityType = CurrentQuestion.SpecialityType;//new
                NewTopicName = CurrentQuestion.TopicName;
                NewQuestionTypeIndex = CurrentQuestion.QuestionType-1;
                NewQuestionMediaLink = CurrentQuestion.MediaLink;
            }

            

        }

        #endregion

        #region Final question
        private void ApplyFinalQuestionChangesMethod(object obj)
        {
            FinalQuestionSetup = new Question(NewFinalQuestionText, NewFinalQuestionAnswer,0,1,true,NewTopicName);
        }

        //ToDo
        private void GetNewFinalQuestionMethod(object obj)
        {
            if (SelectedFinalQuestionIndex != FinalQuestions.Count - 1) SelectedFinalQuestionIndex++;
            else SelectedFinalQuestionIndex = 0;

            FinalQuestionSetup = FinalQuestions[SelectedFinalQuestionIndex];
        }



        #endregion

        #endregion









    }
}
