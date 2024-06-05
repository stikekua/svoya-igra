using SvoyaIgra.Game.Enums;
using SvoyaIgra.Game.ViewModels.Helpers;
using System;

namespace SvoyaIgra.Game.Metadata
{

    public class Question : ViewModelBase
    {
        public string QuestionText { get; set; } = "";
        public string QuestionAnswer { get; set; } = "";
        public string TopicName { get; set; } = "";
        public string MediaLink { get; set; } = "00000000-0000-0000-0000-000000000000";
        public int Price { get; set; } = 0;

        public bool IsSpecial
        {
            get {  return SpecialityType== SpecialityTypesEnum.NotSpecial ? false:true;  }
        }


        private SpecialityTypesEnum _specialityType = SpecialityTypesEnum.NotSpecial;
        public SpecialityTypesEnum SpecialityType
        {
            get { return _specialityType; }
            set
            {
                if (_specialityType != value)
                {
                    _specialityType = value;
                    OnPropertyChanged(nameof(SpecialityType));
                    OnPropertyChanged(nameof(IsSpecial));
                }
            }
        }

        int _specialityCatPrice = 0;
        public int SpecialityCatPrice
        {
            get { return _specialityCatPrice; }
            set
            {
                if (_specialityCatPrice != value)
                {
                    _specialityCatPrice = value;
                    OnPropertyChanged(nameof(NotYetAsked));
                }
            }
        }


        bool _notYetAsked = true;
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


        public QuestionTypeEnum QuestionType { get; set; } = QuestionTypeEnum.Text;
       
        public Question(string questionText, string questionAnswer, int price, QuestionTypeEnum questionType = QuestionTypeEnum.Text, bool notYetAsked = true, string topicName="" )
        {
            QuestionText = questionText;
            QuestionAnswer = questionAnswer;
            Price = price;
            NotYetAsked = notYetAsked;
            QuestionType = questionType;
            TopicName=topicName;
        }
        public Question(string topicName, string questionText, string questionAnswer, int price, QuestionTypeEnum questionType = QuestionTypeEnum.Text, string mediaLink = "00000000-0000-0000-0000-000000000000")
        {
            TopicName = topicName;
            QuestionText = questionText;
            QuestionAnswer = questionAnswer;
            Price = price;
            QuestionType = questionType;
            MediaLink = mediaLink;
        }

        public Question()
        {

        }

    }
}
