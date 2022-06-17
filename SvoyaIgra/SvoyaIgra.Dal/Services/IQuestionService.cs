using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public interface IQuestionService
{
    public Task<QuestionDto?> GetQuestionAsync(int id);
    public Task<QuestionDto?> CreateQuestionAsync(CreateQuestionDto);
    public Task<QuestionDto?> UpdateQuestionAsync(UpdateQuestionDto);
    public Task<QuestionDto?> DeleteQuestionAsync(int id);
}