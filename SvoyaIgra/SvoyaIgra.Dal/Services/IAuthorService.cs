using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public interface IAuthorService
{
    public Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();

    public Task<AuthorDto?> GetAuthorAsync(int id);
    public Task<AuthorDto?> GetAuthorAsync(string name);
    public Task<AuthorDto?> CreateAuthorAsync(string name);
    public Task<AuthorDto?> UpdateAuthorAsync(int id, string name);
    public Task<AuthorDto?> DeleteAuthorAsync(int id);

    public Task<IEnumerable<QuestionDto>?> GetAuthorQuestionsAsync(int authorId);
    
}