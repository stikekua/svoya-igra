using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public interface IThemeService
{
    public Task<ThemeDto?> GetThemeAsync(int id);
    public Task<ThemeDto?> CreateThemeAsync(string name, ThemeDifficulty difficulty);
    public Task<ThemeDto?> UpdateThemeAsync(int id, string name, ThemeDifficulty difficulty);
    public Task<ThemeDto?> DeleteThemeAsync(int id);

    public Task<IEnumerable<ThemeDto>> GetAllThemesAsync();
    public Task<IEnumerable<QuestionDto>?> GetThemeQuestionsAsync(int id); 

}