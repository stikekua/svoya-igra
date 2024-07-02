using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.Dal.Services;

public class TopicService<TContext> : ITopicService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public TopicService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<TopicDto> GetAllTopics()
    {
        var topics = _dbContext.Set<Topic>().ToList();

        return topics.ToList().Select(t => t.ToDto());
    }

    public async Task<TopicDto?> GetTopicAsync(int id)
    {
        var topic = await _dbContext.Set<Topic>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        return topic?.ToDto();
    }

    public async Task<TopicDto?> GetTopicAsync(string name)
    {
        var topic = await _dbContext.Set<Topic>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == name);

        return topic?.ToDto();
    }

    public async Task<TopicDto?> CreateTopicAsync(string name, TopicDifficulty difficulty)
    {
        var topic = new Topic
        {
            Name = name,
            Difficulty = difficulty,
            Lang = "ru",
        };
        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync();
        return topic.ToDto();
    }

    public async Task<TopicDto?> CreateTopicAsync(string name, TopicDifficulty difficulty, string lang)
    {
        var topic = new Topic
        {
            Name = name,
            Difficulty = difficulty,
            Lang = lang,
        };
        _dbContext.Set<Topic>().Add(topic);
        await _dbContext.SaveChangesAsync();
        return topic.ToDto();
    }

    public Task<TopicDto?> UpdateTopicAsync(int id, string name, TopicDifficulty difficulty)
    {
        throw new NotImplementedException();
    }
    public Task<TopicDto?> DeleteTopicAsync(int id)
    {
        throw new NotImplementedException();
    }
}