using SvoyaIgra.Game.ViewModels.Helpers;

namespace SvoyaIgra.Game.Metadata
{
    public class Player : ViewModelBase
    {
        string _name { get; set; }
        public string Name 
        { 
            get { return _name; }
            set 
            {
                if (value!=_name)
                {
                    if (value.Equals(string.Empty))
                    {
                        _name = "NoName";
                    }
                    else _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            
            } 
        }
        int _score = 0;
        public int Score 
        { 
            get
            {
                return _score;
            }
            set
            {
                if (_score != value)
                {
                    _score = value;
                    OnPropertyChanged(nameof(Score));
                }
                
            }
        }
        public string ColorCode { get; set; }
        public bool isActive { get; set; } = true;
        public bool isInQueue { get; set; } = true;
        public bool isSelected { get; set; } = false;

        public Player(string name, string colorCode)
        {
            Name = name;
            ColorCode = colorCode;
        }
    }
}
