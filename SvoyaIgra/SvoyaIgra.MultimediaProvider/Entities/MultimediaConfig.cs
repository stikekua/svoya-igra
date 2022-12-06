namespace SvoyaIgra.MultimediaProvider.Entities;

public class MultimediaConfig
{
    public string FolderPath { get; set; }
    public IEnumerable<string> QuestionFiles { get; set; }
    public IEnumerable<string> AnswerFiles { get; set; }
}