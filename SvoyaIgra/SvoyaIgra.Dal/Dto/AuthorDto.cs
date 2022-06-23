namespace SvoyaIgra.Dal.Dto;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<TopicDto>? Topics { get; set; }
}