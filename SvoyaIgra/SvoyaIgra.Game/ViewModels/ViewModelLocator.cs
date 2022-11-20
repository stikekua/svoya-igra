using Microsoft.Extensions.DependencyInjection;

namespace SvoyaIgra.Game.ViewModels;

public class ViewModelLocator
{
    public GameViewModel GameViewModel
        => App.ServiceProvider.GetRequiredService<GameViewModel>();

    public QuestionsSetupViewModel QuestionsSetupViewModel
        => App.ServiceProvider.GetRequiredService<QuestionsSetupViewModel>();

}