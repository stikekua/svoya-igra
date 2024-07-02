using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.Dal.Bo
{
    public class Topic
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Lang { get; set; }

        public TopicDifficulty Difficulty { get; set; }

        public IEnumerable<Question> Questions { get; set; }
    }
}
