using SvoyaIgra.Game.Metadata;
using SvoyaIgra.Game.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace SvoyaIgra.Game.ViewModels
{
    public class QuestionsSetupViewModel:ViewModelBase
    {
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
                    GetCurrentQuestion();
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
                    GetCurrentQuestion();
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
                    GetCurrentQuestion();
                }
            }
        }

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

        private string _newQuestionAnswer="Some answer";
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

        private string _newCatTopicName = "";
        public string NewCatTopicName
        {
            get { return _newCatTopicName; }
            set
            {
                if (_newCatTopicName != value)
                {
                    _newCatTopicName = value;
                    OnPropertyChanged(nameof(NewCatTopicName));
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
                }
            }
        }


        public RelayCommand ApplyQuestionChangesCommand { get; set; }
        public RelayCommand GetTestQuestionsCommand { get; set; }
        public RelayCommand GetRealQuestionsFromDbCommand { get; set; }
        public RelayCommand GetRandomCatQuestionsCommand { get; set; }


        public QuestionsSetupViewModel(ObservableCollection<ObservableCollection<Topic>> allRoundsQuestions)
        {
            AllRoundsQuestions = allRoundsQuestions;

            ApplyQuestionChangesCommand = new RelayCommand(ApplyQuestionChangesMethod);
            GetTestQuestionsCommand = new RelayCommand(GetTestQuestionsMethod);
            GetRealQuestionsFromDbCommand = new RelayCommand(GetRealQuestionsFromDbMethod);
            GetRandomCatQuestionsCommand = new RelayCommand(GetRandomCatQuestionsMethod);
        }

        private void GetRandomCatQuestionsMethod(object obj)
        {
            Question questionFromDb = new Question();
            NewQuestionText = questionFromDb.QuestionText;
            NewQuestionAnswer = questionFromDb.QuestionAnswer;
            NewQuestionSpecialityTypeIndex = 1;//Cat
            NewCatTopicName = questionFromDb.SpecialityCatTopicName;
        }

        private void GetRealQuestionsFromDbMethod(object obj)
        {
            throw new NotImplementedException();
        }

        private void ApplyQuestionChangesMethod(object obj)
        {
            int newCatPrice = 0;
            if(!NewQuestionAnswer.Equals(string.Empty) && !NewQuestionText.Equals(string.Empty) && Int32.TryParse(NewQuestionSpecialityCatPrice, out newCatPrice))
            {
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionText = NewQuestionText;
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionAnswer = NewQuestionAnswer;                
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityType = NewQuestionSpecialityTypeIndex;
               // AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionType = NewQuestionTypeIndex+1; //conversion between question type in combobox and question type in enum(first index there is 1)
                //AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].MediaLink = NewQuestionMediaLink;

                if (NewQuestionTypeIndex + 1 > (int)QuestionTypeEnum.Text) //if not text
                {
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].MediaLink = NewQuestionMediaLink;
                }
                if (NewQuestionSpecialityTypeIndex==1)
                {                   
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityCatPrice = newCatPrice;
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityCatTopicName = cat;

                }
            }
            OnPropertyChanged(nameof(AllRoundsQuestions));

            MessageBox.Show($"In round {CurrentRoundIndex + 1} in topic {AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Name} Question number {CurrentQuestionIndex + 1} was changed");
        }

        void GetCurrentQuestion()
        {
            if (AllRoundsQuestions.Count>0)
            {
                CurrentQuestion = AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex];
            }

            
        }

        private void GetTestQuestionsMethod(object obj)
        {
            for (int z = 0; z < 3; z++)//rounds
            {
                var topics = new ObservableCollection<Topic>();
                for (int i = 0; i < 6; i++) //topics
                {
                    var listOfQuestions = new List<Question>();
                    for (int k = 0; k < 5; k++) //questions
                    {
                        string questionText = "Topic " + i.ToString() + " question " + k.ToString();
                        listOfQuestions.Add(new Question(questionText, "Answer for " + questionText, (k * 100 + 100) * (z + 1), k + 1));
                    }
                    topics.Add(new Topic(listOfQuestions, "Round " + (z + 1).ToString() + " Topic " + i.ToString()));

                }
                AllRoundsQuestions.Add(topics);
            }
            GetCurrentQuestion();
        }
    }
}
