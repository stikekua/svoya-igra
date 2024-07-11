using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Dal.Services;
using SvoyaIgra.ImportCSV.Services;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.TestClient.Actions;

public class ImportActions:IImportActions
{
    private readonly IImportService _importService;
    private readonly IAuthorService _authorService;
    private readonly ITopicService _topicService;
    private readonly IQuestionService _questionService;

    public ImportActions(IImportService importService, IAuthorService authorService, ITopicService topicService, IQuestionService questionService)
    {
        _importService = importService;
        _authorService = authorService;
        _topicService = topicService;
        _questionService = questionService;
    }

    public void PerformImportAction()
    {

        var fileName = ShowFilesAsync().Result;
        if (fileName == "") return;
        
        ImportFile(fileName).Wait();
    }

    private async Task<string> ShowFilesAsync()
    {
        Ui.Clear();
        Ui.Write("Import");
        Ui.Write("Files found:");
        var files = (await _importService.GetFilesInImportFolder()).ToArray();
        for (int i = 0; i < files.Count(); i++)
        {
            Ui.Write(FormatFile(i+1, files[i]));
        }
        Ui.Write(" 0. <- BACK");
        Ui.Write();
        var selected = Ui.Choice(files.Length);
        if (selected == 0) return "";
        return files[selected-1];
    }

    private string FormatFile(int n, string name)
    {
        return $" {n}. {name,-20} ";
    }

    private async Task ImportFile(string fileName)
    {
        var topics = await _importService.ParseCsvFile(fileName);
        if (topics == null|| !topics.Any())
        {
            Ui.Error("Error during parse file");
            return;
        }
        Ui.Write("File successfully parsed");
        Ui.Write();
        var authorName = Ui.Read("Author name");
        var lang = Ui.ReadString("Language", Language.SupportedLangs);

        var author = await _authorService.GetAuthorAsync(authorName);
        if (author == null)
        {
            author = await _authorService.CreateAuthorAsync(authorName);
            Ui.Write($"Author \"{authorName}\" created.");
        }

        foreach (var topic in topics)
        {
            var t = await _topicService.GetTopicAsync(topic.Name);
            if (t != null)
            {
                Ui.WriteWarning($"Topic \"{topic.Name}\" already exist.");
                continue;
            }

            t = await _topicService.CreateTopicAsync(topic.Name, topic.Difficulty, lang);
            Ui.Write($"Topic \"{t.Name}\" created.");

            if (topic.Questions == null || !topic.Questions.Any()) continue;

            foreach (var question in topic.Questions)
            {
                var q = await _questionService.CreateQuestionAsync(
                    new CreateQuestionRequestDto
                    {
                        Type = question.Type,
                        Difficulty = question.Difficulty,
                        TopicId = t.Id,
                        AuthorId = author.Id,
                        Text = question.Text,
                        MultimediaId = question.MultimediaId ?? Guid.Empty.ToString(),
                        Answer = question.Answer
                    }
                );
                Ui.Write($"Question \"{q.Id}\" created.");
            }

        }

        Ui.PressKey();
    }
}