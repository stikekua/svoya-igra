using SvoyaIgra.Game.ViewModels.Helpers;
using SvoyaIgra.Game.Views;
using SvoyaIgra.Game.Views.GamePhases;
using System;
using System.Diagnostics;
using System.Windows;

namespace SvoyaIgra.Game.ViewModels
{
    public class GameViewModel:ViewModelBase
    {
        #region Properties

        #region Cockpit window control

        public RelayCommand CloseAppCommand { get; set; }



        enum GamePhaseEnum
        {
            GameIntro           = 0,
            FirstRoundIntro     = 1,
            FirstRound          = 2,
            SecondRoundIntro    = 3,
            SecondRound         = 4,
            ThirdRoundIntro     = 5,
            ThirdRound          = 6,
            FinalRoundIntro     = 7,
            FinalRound          = 8,
            Results             = 9
        }


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

        private WindowState _playScreenWindowState = WindowState.Normal;
        public WindowState PlayScreenWindowState
        {
            get { return _playScreenWindowState; }
            set
            {
                if (_playScreenWindowState != value)
                {
                    _playScreenWindowState = value;
                    OnPropertyChanged(nameof(PlayScreenWindowState));
                }
            }
        }

        #region Game phase

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
                    OnPropertyChanged(nameof(IsGameIntro));
                    OnPropertyChanged(nameof(IsFirstRoundIntro));
                    OnPropertyChanged(nameof(IsFirstRound));
                    OnPropertyChanged(nameof(IsSecondRoundIntro));
                    OnPropertyChanged(nameof(IsSecondRound));
                    OnPropertyChanged(nameof(IsThirdRoundIntro));
                    OnPropertyChanged(nameof(IsThirdRound));
                    OnPropertyChanged(nameof(IsFinalRoundIntro));
                    OnPropertyChanged(nameof(IsFinalRound));
                }
            }
        }

        public bool IsGameIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.GameIntro ? true : false; }
        }

        public bool IsFirstRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.FirstRoundIntro ? true : false; }
        }

        public bool IsFirstRound
        {
            get { return GamePhase == (int)GamePhaseEnum.FirstRound ? true : false; }
        }

        public bool IsSecondRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.SecondRoundIntro ? true : false; }
        }

        public bool IsSecondRound
        {
            get { return GamePhase == (int)GamePhaseEnum.SecondRound ? true : false; }
        }

        public bool IsThirdRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.ThirdRoundIntro ? true : false; }
        }
        public bool IsThirdRound
        {
            get { return GamePhase == (int)GamePhaseEnum.ThirdRound ? true : false; }
        }
        public bool IsFinalRoundIntro
        {
            get { return GamePhase == (int)GamePhaseEnum.FinalRoundIntro ? true : false; }        }

        public bool IsFinalRound
        {
            get { return GamePhase == (int)GamePhaseEnum.FinalRound ? true : false; }
        }
        #endregion




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
        public RelayCommand ChangeGamePhaseCommand { get; set; }


        

        #endregion

        #endregion

        #endregion

        public GameViewModel()
        {
          CloseAppCommand  = new RelayCommand(CloseAppMethod);
          OpenPresentScreenCommand = new RelayCommand(OpenPresentScreenMethod);
          ClosePresentScreenCommand = new RelayCommand(ClosePresentScreenMethod);

          ChangeGamePhaseCommand = new RelayCommand(ChangeGamePhaseMethod);

        }

        private void ChangeGamePhaseMethod(object obj)
        {
            int phase = (int)obj;
            if (PlayScreenWindow != null)
            {
                GamePhase = phase;
            }
        }

        #region Methods

        private void LockPresentScreenMethod(bool parameter)
        {
            if (PlayScreenWindow != null)
            {
                if (parameter)
                {
                    PlayScreenWindowState = WindowState.Maximized;
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

        private void ClosePresentScreenMethod(object obj)
        {
            if (PlayScreenWindow != null)
            {
                PlayScreenWindow.Close();
            }
        }

        private void OpenPresentScreenMethod(object obj)
        {
            PlayScreenWindow  = new PlayScreenWindow();
           // PlayScreenWindow.DataContext = PlayScreenViewModel;
            PlayScreenWindow.DataContext = this;
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
