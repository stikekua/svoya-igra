namespace SvoyaIgra.Dal.Bo
{
    public class Topic
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public string Name { get; set; }

        public TopicDifficulty Difficulty { get; set; }

        public IEnumerable<Question> Questions { get; set; }
    }
}
