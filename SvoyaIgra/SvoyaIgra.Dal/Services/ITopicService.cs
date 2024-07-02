using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Shared.Entities;
using System.Xml.Linq;

namespace SvoyaIgra.Dal.Services;

public interface ITopicService
{
    public Task<TopicDto?> GetTopicAsync(int id);
    public Task<TopicDto?> GetTopicAsync(string name);
    public Task<TopicDto?> CreateTopicAsync(string name, TopicDifficulty difficulty);
    public Task<TopicDto?> CreateTopicAsync(string name, TopicDifficulty difficulty, string lang);
    public Task<TopicDto?> UpdateTopicAsync(int id, string name, TopicDifficulty difficulty);
    public Task<TopicDto?> DeleteTopicAsync(int id);

    public IEnumerable<TopicDto> GetAllTopics();
}