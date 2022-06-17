using SvoyaIgra.Dal.Bo;
using System.Text.Json.Serialization;

namespace SvoyaIgra.Dal.Dto
{
    public class TopicDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TopicDifficulty Difficulty { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IEnumerable<Question>? Questions { get; set; }
    }
}
