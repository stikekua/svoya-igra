using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.Dal.Services;

public class GameService<TContext> : IGameService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public GameService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TopicDto>> GetTopicsAsync()
    {
        var topics = new List<TopicDto>();
        
        topics.AddRange(_dbContext.Set<Topic>()
            .Where(t => t.Difficulty == TopicDifficulty.Round)
            .OrderBy(x => Guid.NewGuid()).Take(18)
            .Select(t => t.ToDto())
        );

        var qLevels = new[] { 
            QuestionDifficulty.Level1, 
            QuestionDifficulty.Level2, 
            QuestionDifficulty.Level3,
            QuestionDifficulty.Level4, 
            QuestionDifficulty.Level5
        };

        foreach (var topic in topics)
        {
            var questions = new List<QuestionDto>();
            foreach (var questionDifficulty in qLevels)
            {
                questions.Add(_dbContext.Set<Question>()
                    .Where(q => q.TopicId == topic.Id && q.Difficulty == questionDifficulty)
                    .OrderBy(x => Guid.NewGuid()).Take(1)
                    .First().ToDto()
                    );
            }
            topic.Questions = questions;
        }

        return topics;
    }
    
    public async Task<IEnumerable<TopicDto>> GetTopicsFinalAsync()
    {
        var topics = new List<TopicDto>();
        topics.AddRange(_dbContext.Set<Topic>()
            .Where(t => t.Difficulty == TopicDifficulty.Final)
            .OrderBy(x => Guid.NewGuid()).Take(6)
            .Select(t => t.ToDto())
        );

        foreach (var topic in topics)
        {
            var questions = new List<QuestionDto>();
            questions.Add(_dbContext.Set<Question>()
                .Where(q => q.TopicId == topic.Id && q.Difficulty == QuestionDifficulty.LevelFinal)
                .OrderBy(x => Guid.NewGuid()).Take(1)
                .First().ToDto()
            );
            topic.Questions = questions;
        }

        return topics;
    }
}