﻿using SvoyaIgra.Game.Metadata;
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

        #region Properties

        public GameViewModel GameViewModelInstance { get; set; }


        //ToDo
        #region DB model



        #endregion

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
                    if (GameViewModelInstance!=null)
                    {
                        GameViewModelInstance.AllRoundsQuestions = AllRoundsQuestions;
                    }
                    
                }
            }
        }

        public RelayCommand GetTestQuestionsCommand { get; set; }
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

        public RelayCommand GetRandomCatQuestionsCommand { get; set; }
        public RelayCommand ApplyQuestionChangesCommand { get; set; }

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
                    if (GameViewModelInstance!=null)
                    {
                        GameViewModelInstance.FinalQuestion = FinalQuestionSetup;
                    }
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

        public RelayCommand ApplyFinalQuestionChangesCommand { get; set; }
        public RelayCommand GetNewFinalQuestionCommand { get; set; }

        #endregion

        #endregion

        public QuestionsSetupViewModel(GameViewModel vm)
        {
            GameViewModelInstance = vm;

            ApplyQuestionChangesCommand = new RelayCommand(ApplyQuestionChangesMethod);
            GetTestQuestionsCommand = new RelayCommand(GetTestQuestionsMethod);
            GetRealQuestionsFromDbCommand = new RelayCommand(GetRealQuestionsFromDbMethod);
            GetRandomCatQuestionsCommand = new RelayCommand(GetRandomCatQuestionsMethod);
            ApplyFinalQuestionChangesCommand = new RelayCommand(ApplyFinalQuestionChangesMethod);
            GetNewFinalQuestionCommand = new RelayCommand(GetNewFinalQuestionMethod);
        }

        #region Methods

        #region All questions

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
                        listOfQuestions.Add(new Question(questionText, "Answer for " + questionText, (k * 100 + 100) * (z + 1), k + 1,true, "Topic " + i.ToString()));
                    }
                    topics.Add(new Topic(listOfQuestions, "Round " + (z + 1).ToString() + " Topic " + i.ToString()));

                }
                AllRoundsQuestions.Add(topics);
            }


            FinalQuestionSetup = new Question("some final question text", "some final question answer", 0,1,true,"Just Final question");


            GetCurrentQuestion();
        }

        //ToDo
        private void GetRealQuestionsFromDbMethod(object obj)
        {
            throw new NotImplementedException();
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

        //ToDo
        private void GetRandomCatQuestionsMethod(object obj)
        {
            Question questionFromDb = new Question(null, null, 0);
            NewQuestionText = questionFromDb.QuestionText;
            NewQuestionAnswer = questionFromDb.QuestionAnswer;
            NewQuestionSpecialityTypeIndex = 1;//Cat
            NewTopicName = questionFromDb.TopicName;
        }
        private void ApplyQuestionChangesMethod(object obj)
        {
            int newCatPrice = 0;
            if (!NewQuestionAnswer.Equals(string.Empty) && !NewQuestionText.Equals(string.Empty) && Int32.TryParse(NewQuestionSpecialityCatPrice, out newCatPrice))
            {
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionText = NewQuestionText;
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].QuestionAnswer = NewQuestionAnswer;
                AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityType = NewQuestionSpecialityTypeIndex;

                if (NewQuestionTypeIndex + 1 > (int)QuestionTypeEnum.Text) //if not text
                {
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].MediaLink = NewQuestionMediaLink;
                }
                if (NewQuestionSpecialityTypeIndex == 1)
                {
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].SpecialityCatPrice = newCatPrice;
                    AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Questions[CurrentQuestionIndex].TopicName = NewTopicName;

                }
            }
            OnPropertyChanged(nameof(AllRoundsQuestions));

            MessageBox.Show($"In round {CurrentRoundIndex + 1} in topic {AllRoundsQuestions[CurrentRoundIndex][CurrentTopicIndex].Name} Question number {CurrentQuestionIndex + 1} was changed");
        }

        #endregion

        #region Final question
        private void ApplyFinalQuestionChangesMethod(object obj)
        {
            FinalQuestionSetup = new Question(NewFinalQuestionText, NewFinalQuestionAnswer,0,1,true,NewTopicName);
            GameViewModelInstance.FinalQuestion = FinalQuestionSetup;            
        }

        //ToDo
        private void GetNewFinalQuestionMethod(object obj)
        {

        }



        #endregion

        #endregion









    }
}
