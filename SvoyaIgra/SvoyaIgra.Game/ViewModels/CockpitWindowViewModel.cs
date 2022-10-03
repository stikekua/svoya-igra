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
        public PlayScreenWindow PlayScreenWindow { get; set; }
        public PlayScreenViewModel PlayScreenViewModel { get; set; }

        #region RelayCommands
        public RelayCommand OpenPresentScreenCommand { get; set; } 
        public RelayCommand ClosePresentScreenCommand { get; set; } 
        public RelayCommand MinimizePresentScreenCommand { get; set; } 
        public RelayCommand MaximizePresentScreenCommand { get; set; }
        public RelayCommand MovePresentScreenCommand { get; set; }

        #endregion

        #endregion

        #endregion

        public CockpitWindowViewModel()
        {
          CloseAppCommand  = new RelayCommand(CloseAppMethod);
          OpenPresentScreenCommand = new RelayCommand(OpenPresentScreenMethod);
          ClosePresentScreenCommand = new RelayCommand(ClosePresentScreenMethod);
          MinimizePresentScreenCommand = new RelayCommand(MinimizePresentScreenMethod);
          MaximizePresentScreenCommand = new RelayCommand(MaximizePresentScreenMethod);
          MovePresentScreenCommand = new RelayCommand(MovePresentScreenMethod);


        }

        #region Methods

        private void MovePresentScreenMethod(object obj)
        {
            if (PlayScreenWindow != null && PlayScreenViewModel != null)
            {
               // PlayScreenWindow.;  working on it
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
            PlayScreenWindow.Show();
            
        }


        private void CloseAppMethod(object obj)
        {
            App.Current.Shutdown();
        }

        #endregion
    }
}
