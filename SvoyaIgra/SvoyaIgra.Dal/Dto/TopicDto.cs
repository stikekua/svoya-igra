using System.Text.Json.Serialization;
using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.Dal.Dto
{
    public class TopicDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TopicDifficulty Difficulty { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IEnumerable<QuestionDto>? Questions { get; set; }
    }
}
