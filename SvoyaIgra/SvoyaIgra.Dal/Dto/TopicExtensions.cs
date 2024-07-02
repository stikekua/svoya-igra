using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.Dal.Dto
{
    static class TopicExtensions
    {
        public static TopicDto ToDto(this Topic topic)
        {
            return new TopicDto
            {
                Id = topic.Id,
                Name = topic.Name,
                Lang = topic.Lang,
                Difficulty = topic.Difficulty
            };
        }
    }
}
