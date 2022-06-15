using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.Dal.Dto
{
    static class QuestionExtension
    {
        public static QuestionDto ToDto(this Question question)
        {
            return new QuestionDto
            {
                Id = question.Id,
                Type = question.Type,
                Difficulty = question.Difficulty,
                ThemeId = question.ThemeId,
                Text = question.Text,
                MultimediaId = question.MultimediaId == Guid.Empty.ToString() ? null : question.MultimediaId,
                Answer = question.Answer
            };
        }
    }
}
