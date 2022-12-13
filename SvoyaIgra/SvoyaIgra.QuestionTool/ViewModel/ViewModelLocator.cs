using Microsoft.Extensions.DependencyInjection;

namespace SvoyaIgra.QuestionTool.ViewModel;

public class ViewModelLocator
{
    public MainViewModel MainViewModel
        => App.ServiceProvider.GetRequiredService<MainViewModel>();

    public DashboardViewModel DashboardViewModel
        => App.ServiceProvider.GetRequiredService<DashboardViewModel>();

    public AuthorsViewModel AuthorsViewModel
        => App.ServiceProvider.GetRequiredService<AuthorsViewModel>();
    public TopicsViewModel TopicsViewModel
        => App.ServiceProvider.GetRequiredService<TopicsViewModel>();
    public QuestionsViewModel QuestionsViewModel
        => App.ServiceProvider.GetRequiredService<QuestionsViewModel>();
    public ImportViewModel ImportViewModel
        => App.ServiceProvider.GetRequiredService<ImportViewModel>();

}