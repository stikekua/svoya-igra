using SvoyaIgra.Game.ViewModels.Helpers;
using SvoyaIgra.Game.Views;
using System;
using System.Diagnostics;
using System.Windows;

namespace SvoyaIgra.Game.ViewModels
{
    public class CockpitWindowViewModel:ViewModelBase
    {
        #region Properties

        #region Cockpit window control

        public RelayCommand CloseAppCommand { get; set; }



        #endregion

        #region PlayScreen

        PlayScreenWindow _playScreenWindow=null;
        public PlayScreenWindow PlayScreenWindow 
        { 
            get
            {
                return _playScreenWindow;
            }
            set
            {
                if (_playScreenWindow!=value)
                {
                    _playScreenWindow = value;
                    OnPropertyChanged(nameof(PlayScreenWindow));
                    OnPropertyChanged(nameof(PlayScreenRunning));
                }
            }
        }

        public PlayScreenViewModel PlayScreenViewModel { get; set; }

        public bool PlayScreenRunning
        {
            get
            {
               if (PlayScreenWindow!=null) return true;
               return false;
            }
        }

        bool _playScreenLocked = false;
        public bool PlayScreenLocked
        {
            get
            {
                return _playScreenLocked;
            }
            set
            {
                if (_playScreenLocked != value)
                {
                    _playScreenLocked = value;
                    OnPropertyChanged(nameof(PlayScreenLocked));
                    LockPresentScreenMethod(value);

                }
            }
        }


        #region RelayCommands
        public RelayCommand OpenPresentScreenCommand { get; set; } 
        public RelayCommand ClosePresentScreenCommand { get; set; } 
        public RelayCommand MinimizePresentScreenCommand { get; set; } 
        public RelayCommand MaximizePresentScreenCommand { get; set; }
        public RelayCommand ChangeGamePhaseCommand { get; set; }


        

        #endregion

        #endregion

        #endregion

        public CockpitWindowViewModel()
        {
          CloseAppCommand  = new RelayCommand(CloseAppMethod);
          OpenPresentScreenCommand = new RelayCommand(OpenPresentScreenMethod);
          ClosePresentScreenCommand = new RelayCommand(ClosePresentScreenMethod);

            ChangeGamePhaseCommand = new RelayCommand(ChangeGamePhaseMethod);

        }

        private void ChangeGamePhaseMethod(object obj)
        {
            if (PlayScreenWindow != null && PlayScreenViewModel != null)
            {
                if (PlayScreenViewModel.GamePhase == 0) PlayScreenViewModel.GamePhase = 1;
                else PlayScreenViewModel.GamePhase = 0;
            }
        }

        #region Methods

        private void LockPresentScreenMethod(bool parameter)
        {
            if (PlayScreenWindow != null && PlayScreenViewModel != null)
            {
                if (parameter)
                {
                    PlayScreenViewModel.WindowState = WindowState.Maximized;
                    PlayScreenWindow.Topmost = true;
                    PlayScreenWindow.WindowStyle = WindowStyle.None;
                }
                else
                {
                    PlayScreenWindow.Topmost = false;
                    PlayScreenWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                }
            }
                

        }

        private void MinimizePresentScreenMethod(object obj)
        {
            if (PlayScreenWindow!=null && PlayScreenViewModel!=null)
            {
                PlayScreenViewModel.WindowState = WindowState.Minimized;
            }
        }

        private void MaximizePresentScreenMethod(object obj)
        {
            if (PlayScreenWindow != null && PlayScreenViewModel != null)
            {
                PlayScreenViewModel.WindowState = WindowState.Maximized;
            }
        }

        private void ClosePresentScreenMethod(object obj)
        {
            if (PlayScreenWindow != null && PlayScreenViewModel != null)
            {
                PlayScreenWindow.Close();
            }
        }

        private void OpenPresentScreenMethod(object obj)
        {
            PlayScreenViewModel = new PlayScreenViewModel();
            PlayScreenWindow  = new PlayScreenWindow();
            PlayScreenWindow.DataContext = PlayScreenViewModel;
            PlayScreenWindow.WindowState = WindowState.Maximized;
            PlayScreenWindow.Show();            
        }


        private void CloseAppMethod(object obj)
        {
            App.Current.Shutdown();
        }

        #endregion
    }
}
