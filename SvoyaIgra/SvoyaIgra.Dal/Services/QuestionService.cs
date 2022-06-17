using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public class QuestionService<TContext> : IQuestionService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public QuestionService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<QuestionDto?> GetQuestionAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionDto?> CreateQuestionAsync(CreateQuestionRequestDto request)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionDto?> UpdateQuestionAsync(UpdateQuestionRequestDto request)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionDto?> DeleteQuestionAsync(int id)
    {
        throw new NotImplementedException();
    }
}