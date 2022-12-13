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

        DashboardView(null);
    }

    [RelayCommand]
    private void DashboardView(object obj)
    {
        CurrentView = ViewModelLocator.DashboardViewModel;
        Caption = "Dashboard";
        Icon = IconChar.Home;
    }

    [RelayCommand]
    private void AuthorsView(object obj)
    {
        CurrentView = ViewModelLocator.AuthorsViewModel;
        Caption = "Authors";
        Icon = IconChar.User;
    }

    [RelayCommand]
    private void TopicsView(object obj)
    {
        CurrentView = ViewModelLocator.TopicsViewModel;
        Caption = "Topics";
        Icon = IconChar.Folder;
    }

    [RelayCommand]
    private void QuestionsView(object obj)
    {
        CurrentView = ViewModelLocator.QuestionsViewModel;
        Caption = "Questions";
        Icon = IconChar.QuestionCircle;
    }

    [RelayCommand]
    private void ImportView(object obj)
    {
        CurrentView = ViewModelLocator.ImportViewModel;
        Caption = "Import";
        Icon = IconChar.FileImport;
    }
}