using Microsoft.Extensions.DependencyInjection;

namespace SvoyaIgra.MultimediaViewer.ViewModel;

public class ViewModelLocator
{
    public MainViewModel MainViewModel
        => App.ServiceProvider.GetRequiredService<MainViewModel>();

    public ViewerViewModel ViewerViewModel
        => App.ServiceProvider.GetRequiredService<ViewerViewModel>();

    public EditorViewModel EditorViewModel
        => App.ServiceProvider.GetRequiredService<EditorViewModel>();
}