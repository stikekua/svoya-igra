using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;

namespace SvoyaIgra.QuestionTool.ViewModel;

[INotifyPropertyChanged]
public partial class MainViewModel
{
    [ObservableProperty]
    private object _currentView;
    [ObservableProperty]
    private IconChar _icon;
    [ObservableProperty]
    private string _caption;

    public ViewModelLocator ViewModelLocator { get; set; }
    

    public MainViewModel()
    {
        ViewModelLocator = new ViewModelLocator();
        //CurrentView = ViewModelLocator.ViewerViewModel;
        Caption = "Dashboard";
        Icon = IconChar.Home;
    }

    //[RelayCommand]
    //private void ViewerView(object obj)
    //{
    //    CurrentView = ViewModelLocator.ViewerViewModel;
    //}

    //[RelayCommand]
    //private void EditorView(object obj)
    //{
    //    CurrentView = ViewModelLocator.EditorViewModel;
    //}
}