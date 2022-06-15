using SvoyaIgra.Dal.Bo;
using System.Text.Json.Serialization;

namespace SvoyaIgra.Dal.Dto
{
    public class ThemeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ThemeDifficulty Difficulty { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IEnumerable<Question> Questions { get; set; }
    }
}
