namespace SvoyaIgra.Dal.Bo
{
    public class Question
    {
        public int Id { get; set; }
        public QuestionType Type { get; set; }
        public QuestionDifficulty Difficulty { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string Text { get; set; }
        public string MultimediaId { get; set; }
        public string Answer { get; set; }
    }
}
