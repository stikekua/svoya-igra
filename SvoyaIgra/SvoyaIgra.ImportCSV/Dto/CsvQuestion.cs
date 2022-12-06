namespace SvoyaIgra.ImportCSV.Dto;

public class CsvQuestion
{
    //Round;TopicNo;Topic;QuestionDifficulty;QuestionType;Question;Answer;MultimediaId

    public int Round { get; set; }
    public int TopicNo { get; set; }
    public string Topic { get; set; }
    public int QuestionDifficulty { get; set; }
    public string QuestionType { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public string? MultimediaId { get; set; }

}