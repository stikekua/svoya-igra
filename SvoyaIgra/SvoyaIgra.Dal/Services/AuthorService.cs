using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public class AuthorService<TContext> : IAuthorService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public AuthorService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        var authors = _dbContext.Set<Author>()
            .AsNoTracking()
            .ToList();

        return authors.Select(t => t.ToDto());
    }

    public async Task<AuthorDto?> GetAuthorAsync(int id)
    {
        var author = await _dbContext.Set<Author>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        return author?.ToDto();
    }

    public async Task<AuthorDto?> CreateAuthorAsync(string name)
    {
        var author = new Author
        {
            Name = name
        };
        _dbContext.Set<Author>().Add(author);
        await _dbContext.SaveChangesAsync();
        return author.ToDto();
    }

    public Task<AuthorDto?> UpdateAuthorAsync(int id, string name)
    {
        throw new NotImplementedException();
    }

    public Task<AuthorDto?> DeleteAuthorAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TopicDto>?> GetAuthorTopicsAsync(int authorId)
    {
        var topics = _dbContext.Set<Topic>()
            .Where(t => t.AuthorId == authorId)
            .AsNoTracking();

        return topics.Select(t => t.ToDto());
    }
}