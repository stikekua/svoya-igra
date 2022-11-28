using SvoyaIgra.Game.ViewModels.Helpers;

namespace SvoyaIgra.Game.Metadata
{
    public class Player : ViewModelBase
    {
        public string Name { get; set; }
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
        public bool isSelected { get; set; } = false;

        public Player(string name, string colorCode)
        {
            Name = name;
            ColorCode = colorCode;
        }
    }
}
