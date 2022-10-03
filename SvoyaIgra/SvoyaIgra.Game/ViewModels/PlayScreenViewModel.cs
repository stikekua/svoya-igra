using SvoyaIgra.Game.ViewModels.Helpers;
using System.Windows;

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



        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion


    }
}
