using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.Dal.Services;

public interface ITopicService
{
    public Task<TopicDto?> GetTopicAsync(int id);
    public Task<TopicDto?> GetTopicAsync(string name);
    public Task<TopicDto?> CreateTopicAsync(string name, TopicDifficulty difficulty);
    public Task<TopicDto?> UpdateTopicAsync(int id, string name, TopicDifficulty difficulty);
    public Task<TopicDto?> DeleteTopicAsync(int id);

    public Task<IEnumerable<TopicDto>> GetAllTopicsAsync();
}