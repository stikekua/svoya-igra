using SvoyaIgra.Game.ViewModels.Helpers;
using System.Windows;
using System.Windows.Navigation;

namespace SvoyaIgra.Game.ViewModels
{
    public class PlayScreenViewModel:ViewModelBase
    {
        #region Properties
        private WindowState _windowState = WindowState.Normal;
        public WindowState WindowState
        {
            get { return _windowState; }
            set 
            {
                if (_windowState!=value)
                {
                    _windowState=value;
                    OnPropertyChanged(nameof(WindowState));
                }
            }
        }

        private int _gamePhase = 0;
        public int GamePhase
        {
            get { return _gamePhase; }
            set
            {
                if (_gamePhase != value)
                {
                    _gamePhase = value;
                    OnPropertyChanged(nameof(GamePhase));
                    OnPropertyChanged(nameof(IsFirstRoundIntro));
                    OnPropertyChanged(nameof(IsNormalRound));
                }
            }
        }


        public bool IsFirstRoundIntro
        {
            get { return GamePhase==0 ? true:false; }
        }

        public bool IsNormalRound
        {
            get { return GamePhase == 1 ? true : false; }
        }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion


    }
}
