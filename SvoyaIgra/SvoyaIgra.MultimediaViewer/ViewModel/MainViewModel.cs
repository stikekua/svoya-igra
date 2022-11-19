using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SvoyaIgra.MultimediaViewer.ViewModel;

[INotifyPropertyChanged]
public partial class MainViewModel
{
    [ObservableProperty]
    private object _currentView;

    public ViewModelLocator ViewModelLocator { get; set; }
    

    public MainViewModel()
    {
        var ViewModelLocator = new ViewModelLocator();
        CurrentView = ViewModelLocator.ViewerViewModel;
    }

    [RelayCommand]
    private void ViewerView(object obj)
    {
        CurrentView = ViewModelLocator.ViewerViewModel;
    }

    [RelayCommand]
    private void EditorView(object obj)
    {
        CurrentView = ViewModelLocator.EditorViewModel;
    }
}