using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.Dal.Dto
{
    static class ThemeExtensions
    {
        public static ThemeDto ToDto(this Theme theme)
        {
            return new ThemeDto
            {
                Id = theme.Id,
                Name = theme.Name,
                Difficulty = theme.Difficulty
            };
        }
    }
}
