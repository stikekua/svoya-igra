using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public class QuestionService<TContext> : IQuestionService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public QuestionService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<QuestionDto?> GetQuestionAsync(int id)
    {
        var questions = await _dbContext.Set<Question>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        return questions?.ToDto();
    }

    public async Task<QuestionDto?> CreateQuestionAsync(CreateQuestionRequestDto request)
    {
        var question = new Question
        {
            Type = request.Type,
            Difficulty = request.Difficulty,
            TopicId = request.TopicId,
            AuthorId = request.AuthorId,
            Text = request.Text,
            MultimediaId = request.MultimediaId,
            Answer = request.Answer
        };
        _dbContext.Set<Question>().Add(question);
        await _dbContext.SaveChangesAsync();
        return question.ToDto();
    }

    public async Task<QuestionDto?> SetQuestionMultimediaIdAsync(int id, string multimediaId)
    {
        var question = await _dbContext.Set<Question>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        question.MultimediaId = multimediaId;
        _dbContext.Set<Question>().Update(question);
        await _dbContext.SaveChangesAsync();
        return question.ToDto();
    }

    public Task<QuestionDto?> UpdateQuestionAsync(UpdateQuestionRequestDto request)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionDto?> DeleteQuestionAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<QuestionDto>?> GetQuestionsByAuthorAsync(int authorId)
    {
        var questions = _dbContext.Set<Question>()
            .Where(q => q.AuthorId == authorId)
            .AsNoTracking();

        return questions.Select(q => q.ToDto());
    }

    public async Task<IEnumerable<QuestionDto>?> GetQuestionsByTopicAsync(int topicId)
    {
        var questions = _dbContext.Set<Question>()
            .Where(q => q.TopicId == topicId)
            .AsNoTracking();

        return questions.Select(q => q.ToDto());
    }
}