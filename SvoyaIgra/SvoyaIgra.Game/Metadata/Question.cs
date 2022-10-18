using SvoyaIgra.Game.ViewModels.Helpers;

namespace SvoyaIgra.Game.Metadata
{
    public class Question : ViewModelBase
    {
        public string QuestionText { get; set; } = "";
        public string QuestionAnswer { get; set; } = "";
        public int Price { get; set; } = 0;
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
        public int QuestionType { get; set; } = 1;

        //Text = 1
        //Picture = 2
        //PictureSeries = 3
        //Musical = 4
        //Video = 5
       
        public Question(string questionText, string questionAnswer, int price, int questionType = 1, bool notYetAsked = true )
        {
            QuestionText = questionText;
            QuestionAnswer = questionAnswer;
            Price = price;
            NotYetAsked = notYetAsked;
            QuestionType = questionType;
        }

        public Question()
        {

        }



    }
}
