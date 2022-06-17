using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.TestClient.Theme;

public class ThemeActions : IThemeActions
{
    private readonly IThemeService _themeService;

    public ThemeActions(IThemeService themeService)
    {
        _themeService = themeService;
    }

    public void PerformThemeAction()
    {
        do
        {
            var index = ThemeMenu();
            switch (index)
            {
                case 0:
                    return;
                case 1:
                    ListThemesAsync().Wait();
                    break;
                case 2:
                    ListThemeByIdAsync().Wait();
                    break;
                case 3:
                    CreateThemeAsync().Wait();
                    break;
                case 4:
                    //SetUserStatusAsync(true).Wait();
                    break;
                case 5:
                    //ResetPasswordAsync().Wait();
                    break;
                default:
                    continue;
            }
        } while (true);
    }

    private static int ThemeMenu()
    {
        Ui.Clear();
        Ui.Write("Theme");
        Ui.Write("Select action:");
        Ui.Write(" 1. List all themes");
        Ui.Write(" 2. List theme");
        Ui.Write(" 3. Create");
        Ui.Write(" 4. ");
        Ui.Write(" 5. ");
        Ui.Write(" 0. <- BACK");
        return Ui.Choice(5);
    }

    private async Task ListThemesAsync()
    {
        Ui.Clear();
        Ui.Write("Theme -> List all themes");
        var themes = await _themeService.GetAllThemesAsync();
        Ui.Write(FormatTheme("Name", "Difficulty"));
        foreach (var theme in themes)
        {
            Ui.Write(FormatTheme(theme.Name, theme.Difficulty.ToString()));
        }
        Ui.Write();
        Ui.PressKey();
    }

    private string FormatTheme(string name, string difficulty)
    {
        return $"{name,-20} {difficulty,-20}";
    }

    private async Task ListThemeByIdAsync()
    {
        Ui.Clear();
        Ui.Write("Theme -> Show theme details");
        var themeId = Ui.ReadInt("  ThemeId");
        var theme = await _themeService.GetThemeAsync(themeId);
        if (theme == null)
        {
            return;
        }
        Ui.Write();
        Ui.Write(FormatTheme("Name", "Difficulty"));
        Ui.Write(FormatTheme(theme.Name, theme.Difficulty.ToString()));
        Ui.Write();

        var questions = await _themeService.GetThemeQuestionsAsync(themeId);
        Ui.Write("Questions of the theme:");
        if (questions == null || !questions.Any())
        {
            Ui.WriteWarning("\tNo questions found");
            Ui.Write();
            Ui.PressKey();
            return;
        }
        Ui.Write(FormatQuestion("Type", "Difficulty", "Text"));
        foreach (var question in questions)
        {
            Ui.Write(FormatQuestion(question.Type.ToString(), question.Difficulty.ToString(), question.Text));
        }
        Ui.Write();
        Ui.PressKey();
    }

    private string FormatQuestion(string type, string difficulty, string text)
    {
        return $"{type,-20} {difficulty,-20} {text,-20}";
    }

    private async Task CreateThemeAsync()
    {
        Ui.Clear();
        Ui.Write("Theme -> Create theme");
        var name = Ui.Read("Name");
        var allowedValues = ((ThemeDifficulty[])Enum.GetValues(typeof(ThemeDifficulty))).Select(c => (int)c).ToArray();
        var difficulty = Ui.ReadInt("Difficulty", allowedValues);

        var resp = await _themeService.CreateThemeAsync(name, (ThemeDifficulty)difficulty);
        if (resp == null)
        {
            return;
        }
        Ui.Write();
        Ui.Write("Theme successfully created");
        Ui.Write($"Name: {resp.Name}\t\tDifficulty: {resp.Difficulty}");
        Ui.PressKey();
    }
}