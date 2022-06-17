using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public class GameService : IGameService
{
    public Task<IEnumerable<ThemeDto>> GetThemesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ThemeDto>> GetThemesAsync(string[] themeNames)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ThemeDto>> GetThemesFinalAsync()
    {
        throw new NotImplementedException();
    }
}