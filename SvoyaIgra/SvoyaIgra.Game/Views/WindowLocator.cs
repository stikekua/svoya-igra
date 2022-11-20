using Microsoft.Extensions.DependencyInjection;
using SvoyaIgra.Game.ViewModels;
using SvoyaIgra.Game.Views.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.Game.Views;

public class WindowLocator
{
    public PlayScreenWindow PlayScreenWindow
        => App.ServiceProvider.GetRequiredService<PlayScreenWindow>();
    public QuestionsSetupWindow QuestionsSetupWindow
        => App.ServiceProvider.GetRequiredService<QuestionsSetupWindow>();

}

