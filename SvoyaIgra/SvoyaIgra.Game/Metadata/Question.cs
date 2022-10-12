namespace SvoyaIgra.Game.Metadata
{
    public class Question
    {
        public string QuestionText { get; set; }
        public int Price { get; set; }
        public bool NotYetAsked { get; set; }
        public Question(string questionText, int price, bool notYetAsked = true)
        {
            QuestionText = questionText;
            Price = price;
            NotYetAsked = notYetAsked;
        }
    }
}
