namespace SvoyaIgra.Dal.Bo;

public class GameSession
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ParametersConfig { get; set; } //TODO not null
    public string? TopicsConfig { get; set; }
}