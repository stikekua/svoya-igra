using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public interface IQuestionService
{
    public Task<QuestionDto?> GetQuestionAsync(int id);
    public Task<QuestionDto?> CreateQuestionAsync(CreateQuestionRequestDto request);
    public Task<QuestionDto?> UpdateQuestionAsync(UpdateQuestionRequestDto request);
    public Task<QuestionDto?> DeleteQuestionAsync(int id);
}