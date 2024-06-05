using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Dal.Helpers;
using SvoyaIgra.Shared.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.Dal.Services;

public class GameService<TContext> : IGameService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public GameService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateGameAsync()
    {
        var game = new GameSession();
        game.CreatedAt = DateTime.Now;

        //TODO save game parameters

        _dbContext.Set<GameSession>().Add(game);
        await _dbContext.SaveChangesAsync();
        return game.Id;
    }

    public Task<Guid> GetLastGameAsync()
    {
        var game = _dbContext.Set<GameSession>().OrderByDescending(g => g.CreatedAt).FirstOrDefault();
        return Task.FromResult(game?.Id ?? Guid.Empty);
    }

    public async Task<IEnumerable<TopicDto>> GetTopicsAsync(Guid gameId)
    {
        var game = _dbContext.Set<GameSession>().FirstOrDefault(g => g.Id == gameId);
        var topics = new List<TopicDto>();

        if (game == null) return topics; //game does not exist

        var topicConfig = TopicConfigParser.ToObject(game.TopicsConfig);

        if (topicConfig?.RoundTopicIds != null && topicConfig?.RoundTopicIds.Count() == GameConstants.Round.TopicsCount) //topic once generated, return it
        {
            topics.AddRange(_dbContext.Set<Topic>()
                .Where(t => topicConfig.RoundTopicIds.Contains(t.Id))
                .Select(t => t.ToDto())
            );

            var idsList = topicConfig.RoundTopicIds.ToList();
            topics = topics.OrderBy(t => idsList.IndexOf(t.Id)).ToList();
        }
        else //new topic set  
        {
            topics.AddRange(GetValidRoundTopics()
                .OrderBy(x => Guid.NewGuid()).Take(GameConstants.Round.TopicsCount)
                .Select(t => t.ToDto())
            );

            if (topicConfig == null) topicConfig = new TopicConfig();
            topicConfig.RoundTopicIds = topics.Select(t => t.Id);
            game.TopicsConfig = TopicConfigParser.ToString(topicConfig);
            _dbContext.Set<GameSession>().Update(game);
            await _dbContext.SaveChangesAsync();
        }

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

    public async Task<IEnumerable<TopicDto>> GetTopicsFinalAsync(Guid gameId)
    {
        var game = _dbContext.Set<GameSession>().FirstOrDefault(g => g.Id == gameId);
        var topics = new List<TopicDto>();

        if (game == null) return topics; //game does not exist

        var topicConfig = TopicConfigParser.ToObject(game.TopicsConfig);

        if (topicConfig?.FinalTopicIds != null && topicConfig?.FinalTopicIds.Count() == GameConstants.Final.TopicsCount) //topic once generated, return it
        {
            topics.AddRange(_dbContext.Set<Topic>()
                .Where(t => topicConfig.FinalTopicIds.Contains(t.Id))
                .Select(t => t.ToDto())
            );

            var idsList = topicConfig.FinalTopicIds.ToList();
            topics = topics.OrderBy(t => idsList.IndexOf(t.Id)).ToList();
        }
        else
        {
            topics.AddRange(_dbContext.Set<Topic>()
                .Where(t => t.Difficulty == TopicDifficulty.Final)
                .OrderBy(x => Guid.NewGuid()).Take(GameConstants.Final.TopicsCount)
                .Select(t => t.ToDto())
            );

            if (topicConfig == null) topicConfig = new TopicConfig();
            topicConfig.FinalTopicIds = topics.Select(t => t.Id);
            game.TopicsConfig = TopicConfigParser.ToString(topicConfig);
            _dbContext.Set<GameSession>().Update(game);
            await _dbContext.SaveChangesAsync();
        }

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

    public async Task<TopicDto> GetCatQuestionAsync(Guid gameId)
    {
        var game = _dbContext.Set<GameSession>().FirstOrDefault(g => g.Id == gameId);
        if (game == null) return null; //game does not exist

        var topicConfig = TopicConfigParser.ToObject(game.TopicsConfig);
        if (topicConfig?.RoundTopicIds == null) return null; //topics not generated

        var topic = new TopicDto();
        topic = _dbContext.Set<Topic>()
            .Where(t => !topicConfig.RoundTopicIds.Contains(t.Id) && t.Difficulty == TopicDifficulty.Round)
            .OrderBy(x => Guid.NewGuid()).Take(1)
            .First().ToDto();

        var questions = new List<QuestionDto>();
        questions.Add(_dbContext.Set<Question>()
            .Where(q => q.TopicId == topic.Id)
            .OrderBy(x => Guid.NewGuid()).Take(1)
            .First().ToDto()
        );

        topic.Questions = questions;

        return topic;
    }

    private IEnumerable<Topic> GetValidRoundTopics()
    {
        var dbList = _dbContext.Set<Topic>()
            .Where(t => t.Difficulty == TopicDifficulty.Round)
            .ToList()
            .GroupJoin(_dbContext.Set<Question>(),
                topic => topic.Id,
                question => question.TopicId,
                (x, y) => new { Topic = x, Questions = y })
            .Where(t => t.Questions.Count() >= GameConstants.Round.QuestionDifficultiesCount);

        var qs = dbList.Select(x => x.Questions);

        var validTopicIds = new List<int>();
        foreach (var q in qs)
        {
            var topicId = q.First().TopicId;
            var g = q.GroupBy(
                    q => q.Difficulty,
                    q => q.Id,
                    (key, g) => new { Difficulty = key, Ids = g.ToList() })
                .ToList();

            if (g.Count() < GameConstants.Round.QuestionDifficultiesCount) continue;

            if (g.Select(x => x.Ids.Count).All(x => x >= 1))
            {
                validTopicIds.Add(topicId);
            }
        }

        return dbList.Select(x => x.Topic)
            .Where(t => validTopicIds.Contains(t.Id));
    }
}