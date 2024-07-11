using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public interface IQuestionService
{
    public Task<QuestionDto?> GetQuestionAsync(int id);
    public Task<QuestionDto?> CreateQuestionAsync(CreateQuestionRequestDto request);
    public Task<QuestionDto?> UpdateQuestionAsync(UpdateQuestionRequestDto request);
    public Task<QuestionDto?> DeleteQuestionAsync(int id);
    
    public Task<IEnumerable<QuestionDto>?> GetQuestionsByAuthorAsync(int authorId);
    public Task<IEnumerable<QuestionDto>?> GetQuestionsByTopicAsync(int topicId);

    public Task<QuestionDto?> SetQuestionMultimediaIdAsync(int id, string multimediaId);
}