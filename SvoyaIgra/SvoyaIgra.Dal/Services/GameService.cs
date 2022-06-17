using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public class GameService : IGameService
{
    public Task<IEnumerable<TopicDto>> GetTopicsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TopicDto>> GetTopicsAsync(string[] topicNames)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TopicDto>> GetTopicsFinalAsync()
    {
        throw new NotImplementedException();
    }
}