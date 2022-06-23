namespace SvoyaIgra.Dal.Dto;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<QuestionDto>? Questions { get; set; }
}