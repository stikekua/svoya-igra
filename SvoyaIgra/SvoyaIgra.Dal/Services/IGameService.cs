using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services
{
    public interface IGameService
    {
        /// <summary>
        /// returns randomly 18 themes (6 per each round)
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ThemeDto>> GetThemesAsync();

        /// <summary>
        /// returns randomly 18 themes with the specified themes included
        /// </summary>
        /// <param name="themeNames">specified themes names</param>
        /// <returns></returns>
        public Task<IEnumerable<ThemeDto>> GetThemesAsync(string[] themeNames);

        /// <summary>
        /// returns randomly 7 themes for final round
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ThemeDto>> GetThemesFinalAsync();
    }
}
