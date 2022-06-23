using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.TestClient.Actions;

public class AuthorActions : IAuthorActions
{
    private readonly IAuthorService _authorService;
    private readonly IQuestionService _questionService;

    public AuthorActions(IAuthorService authorService, IQuestionService questionService)
    {
        _authorService = authorService;
        _questionService = questionService;
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

        var questions = await _questionService.GetQuestionsByAuthorAsync(authorId);
        Ui.Write("Author's topics:");
        if (questions == null || !questions.Any())
        {
            Ui.WriteWarning("\tNo topics found");
            Ui.Write();
            Ui.PressKey();
            return;
        }
        Ui.Write(FormatQuestion("Type", "Difficulty", "Text"));
        foreach (var question in questions)
        {
            Ui.Write(FormatQuestion(question.Type.ToString(), question.Difficulty.ToString(), question.Text));
        }
        Ui.Write();
        Ui.PressKey();
    }

    private string FormatQuestion(string type, string difficulty, string text)
    {
        return $"{type,-20} {difficulty,-20} {text,-20}";
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