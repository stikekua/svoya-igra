using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public class TopicService<TContext> : ITopicService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public TopicService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TopicDto>> GetAllTopicsAsync()
    {
        var topics = _dbContext.Set<Topic>()
            .AsNoTracking()
            .ToList();

        return topics.Select(t => t.ToDto());
    }

    public async Task<TopicDto?> GetTopicAsync(int id)
    {
        var topic = await _dbContext.Set<Topic>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        return topic?.ToDto();
    }

    public async Task<IEnumerable<QuestionDto>?> GetTopicQuestionsAsync(int topicId)
    {
        var questions = _dbContext.Set<Question>()
            .Where(q => q.TopicId == topicId)
            .AsNoTracking();

        return questions.Select(q => q.ToDto());
    }
    public async Task<TopicDto?> CreateTopicAsync(string name, TopicDifficulty difficulty)
    {
        var topic = new Topic
        {
            Name = name,
            Difficulty = difficulty
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