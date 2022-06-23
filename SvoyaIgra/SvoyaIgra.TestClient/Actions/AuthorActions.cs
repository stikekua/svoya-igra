using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.TestClient.Actions;

public class AuthorActions : IAuthorActions
{
    private readonly IAuthorService _authorService;

    public AuthorActions(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    public void PerformAuthorAction()
    {
        do
        {
            var index = AuthorMenu();
            switch (index)
            {
                case 0:
                    return;
                case 1:
                    ListAuthorsAsync().Wait();
                    break;
                case 2:
                    ListAuthorByIdAsync().Wait();
                    break;
                case 3:
                    CreateAuthorAsync().Wait();
                    break;
                default:
                    continue;
            }
        } while (true);
    }

    private static int AuthorMenu()
    {
        Ui.Clear();
        Ui.Write("Author");
        Ui.Write("Select action:");
        Ui.Write(" 1. List all authors");
        Ui.Write(" 2. List author");
        Ui.Write(" 3. Create author");
        Ui.Write(" 4. ");
        Ui.Write(" 5. ");
        Ui.Write(" 0. <- BACK");
        return Ui.Choice(5);
    }

    private async Task ListAuthorsAsync()
    {
        Ui.Clear();
        Ui.Write("Author -> List all authors");
        var authors = await _authorService.GetAllAuthorsAsync();
        Ui.Write(FormatAuthor("Id","Name"));
        foreach (var author in authors)
        {
            Ui.Write(FormatAuthor(author.Id.ToString(), author.Name));
        }
        Ui.Write();
        Ui.PressKey();
    }

    private string FormatAuthor(string id, string name)
    {
        return $"{id,-20} {name,-20}";
    }

    private async Task ListAuthorByIdAsync()
    {
        Ui.Clear();
        Ui.Write("Author -> Show author details");
        var authorId = Ui.ReadInt("  AuthorId");
        var author = await _authorService.GetAuthorAsync(authorId);
        if (author == null)
        {
            return;
        }
        Ui.Write(); 
        Ui.Write(FormatAuthor("Id", "Name"));
        Ui.Write(FormatAuthor(author.Id.ToString(), author.Name));
        Ui.Write();

        var topics = await _authorService.GetAuthorTopicsAsync(authorId);
        Ui.Write("Author's topics:");
        if (topics == null || !topics.Any())
        {
            Ui.WriteWarning("\tNo topics found");
            Ui.Write();
            Ui.PressKey();
            return;
        }
        Ui.Write(FormatTopic("Name", "Difficulty"));
        foreach (var topic in topics)
        {
            Ui.Write(FormatTopic(topic.Name, topic.Difficulty.ToString()));
        }
        Ui.Write();
        Ui.PressKey();
    }

    private string FormatTopic(string name, string difficulty)
    {
        return $"{name,-20} {difficulty,-20}";
    }

    private async Task CreateAuthorAsync()
    {
        Ui.Clear();
        Ui.Write("Author -> Create author");
        var name = Ui.Read("Name");

        var resp = await _authorService.CreateAuthorAsync(name);
        if (resp == null)
        {
            return;
        }
        Ui.Write();
        Ui.Write("Topic successfully created");
        Ui.Write(FormatAuthor("Id", "Name"));
        Ui.Write(FormatAuthor(resp.Id.ToString(), resp.Name));
        Ui.PressKey();
    }
}