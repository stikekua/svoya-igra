using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.Dal.Dto;

static class AuthorExtensions
{
    public static AuthorDto ToDto(this Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name
        };
    }
}