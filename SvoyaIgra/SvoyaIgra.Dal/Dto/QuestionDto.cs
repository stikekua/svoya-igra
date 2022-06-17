using SvoyaIgra.Dal.Bo;
using System.Text.Json.Serialization;

namespace SvoyaIgra.Dal.Dto
{
    public class QuestionDto
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionDifficulty Difficulty { get; set; }

        public int ThemeId { get; set; }

        public string Text { get; set; }

        public string MultimediaId { get; set; }

        public string Answer { get; set; }
    }

    public class CreateQuestionRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionDifficulty Difficulty { get; set; }

        public int ThemeId { get; set; }

        public string Text { get; set; }

        public string MultimediaId { get; set; }

        public string Answer { get; set; }
    }

    public class UpdateQuestionRequestDto
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionDifficulty Difficulty { get; set; }

        public int ThemeId { get; set; }

        public string Text { get; set; }

        public string MultimediaId { get; set; }

        public string Answer { get; set; }
    }
}
