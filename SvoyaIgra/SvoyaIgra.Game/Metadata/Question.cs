﻿using SvoyaIgra.Game.ViewModels.Helpers;
using SvoyaIgra.Game.ViewModels;

namespace SvoyaIgra.Game.Metadata
{

    public class Question : ViewModelBase
    {
        public string QuestionText { get; set; } = "";
        public string QuestionAnswer { get; set; } = "";
        public string TopicName { get; set; } = "";
        public string MediaLink { get; set; } = "";
        public int Price { get; set; } = 0;

        public bool IsSpecial
        {
            get 
            { 
                return SpecialityType==0? false:true; 
            }
        }
        private int _specialityType  = (int)SpecialityTypesEnum.NotSpecial;
        public int SpecialityType 
        { 
            get { return _specialityType; }
            set 
            {
                if (_specialityType != value)
                {
                    _specialityType = value;
                    OnPropertyChanged(nameof(SpecialityType));
                }                
            }
        }



        public int SpecialityCatPrice { get; set; } = 0;


        bool _notYetAsked;
        public bool NotYetAsked 
        { 
            get { return _notYetAsked; }
            set 
            {
                if (_notYetAsked != value)
                {
                    _notYetAsked = value;
                    OnPropertyChanged(nameof(NotYetAsked));
                }                 
            } 
        }

        bool _specialIntroWasNotPlayed=true;
        public bool SpecialIntroWasNotPlayed
        {
            get { return _specialIntroWasNotPlayed; }
            set
            {
                if (_specialIntroWasNotPlayed != value)
                {
                    _specialIntroWasNotPlayed = value;
                    OnPropertyChanged(nameof(SpecialIntroWasNotPlayed));
                }
            }
        }


        public int QuestionType { get; set; } = (int)QuestionTypeEnum.Text;

        //Text = 1
        //Picture = 2
        //PictureSeries = 3
        //Musical = 4
        //Video = 5
       
        public Question(string questionText, string questionAnswer, int price, int questionType = 1, bool notYetAsked = true, string topicName="" )
        {
            QuestionText = questionText;
            QuestionAnswer = questionAnswer;
            Price = price;
            NotYetAsked = notYetAsked;
            QuestionType = questionType;
            TopicName=topicName;
        }
        public Question()
        {

        }

    }
}