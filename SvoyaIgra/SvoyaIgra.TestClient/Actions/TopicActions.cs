using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Services;
using SvoyaIgra.Shared.Constants;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.TestClient.Actions;

public class TopicActions : ITopicActions
{
    private readonly ITopicService _topicService;
    private readonly IQuestionService _questionService;
    private readonly IGameService _gameService;
    
    public TopicActions(ITopicService topicService, IQuestionService questionService, IGameService gameService)
    {
        _topicService = topicService;
        _questionService = questionService;
        _gameService = gameService;
    }

    public void PerformTopicAction()
    {
        do
        {
            var index = TopicMenu();
            switch (index)
            {
                case 0:
                    return;
                case 1:
                    ListTopicsAsync().Wait();
                    break;
                case 2:
                    ListTopicByIdAsync().Wait();
                    break;
                case 3:
                    CreateTopicAsync().Wait();
                    break;
                case 4:
                    CreateGameAsync().Wait();
                    break;
                case 5:
                    ListGameTopicsAsync().Wait();
                    break;
                case 6:
                    ListGameFinalTopicsAsync().Wait();
                    break;
                case 7:
                    GameGetCatInBagAsync().Wait();
                    break;
                default:
                    continue;
            }
        } while (true);
    }

    private static int TopicMenu()
    {
        Ui.Clear();
        Ui.Write("Topic");
        Ui.Write("Select action:");
        Ui.Write(" 1. List all topics");
        Ui.Write(" 2. List topic");
        Ui.Write(" 3. Create");
        Ui.Write(" 4. Create game");
        Ui.Write(" 5. Game topics");
        Ui.Write(" 6. Game final topics");
        Ui.Write(" 7. Game get cat");
        Ui.Write(" 0. <- BACK");
        return Ui.Choice(7);
    }

    private async Task ListTopicsAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> List all topics");
        var topics = _topicService.GetAllTopics();
        Ui.Write(FormatTopic("Name", "Difficulty", "Lang"));
        foreach (var topic in topics)
        {
            Ui.Write(FormatTopic(topic.Name, topic.Difficulty.ToString(), topic.Lang));
        }
        Ui.Write();
        Ui.PressKey();
    }

    private string FormatTopic(string name, string difficulty, string lang)
    {
        return $"{name,-20} {difficulty,-20} {lang,-20}";
    }

    private async Task ListTopicByIdAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> Show topic details");
        var topicId = Ui.ReadInt("  TopicId");
        var topic = await _topicService.GetTopicAsync(topicId);
        if (topic == null)
        {
            return;
        }
        Ui.Write();
        Ui.Write(FormatTopic("Name", "Difficulty", "Lang"));
        Ui.Write(FormatTopic(topic.Name, topic.Difficulty.ToString(), topic.Lang));
        Ui.Write();

        var questions = await _questionService.GetQuestionsByTopicAsync(topicId);
        Ui.Write("Questions of the topic:");
        if (questions == null || !questions.Any())
        {
            Ui.WriteWarning("\tNo questions found");
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

    private async Task CreateTopicAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> Create topic");
        var name = Ui.Read("Name");
        var lang = Ui.ReadString("Language", Language.SupportedLangs);

        var allowedValues = ((TopicDifficulty[])Enum.GetValues(typeof(TopicDifficulty))).Select(c => (int)c).ToArray();
        var difficulty = Ui.ReadInt("Difficulty", allowedValues);

        var resp = await _topicService.CreateTopicAsync(name, (TopicDifficulty)difficulty, lang);
        if (resp == null)
        {
            return;
        }
        Ui.Write();
        Ui.Write("Topic successfully created");
        Ui.Write($"Name: {resp.Name}\t\tDifficulty: {resp.Difficulty}\t\tLang: {resp.Lang}");
        Ui.PressKey();
    }

    private async Task CreateGameAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> Create game");
        var lang = Ui.ReadString("Language", Language.SupportedLangs);
        var gameId = await _gameService.CreateGameAsync(lang);
        Ui.Write();
        Ui.Write("Game created:");
        Ui.Write(gameId.ToString());
        Ui.Write();
        Ui.PressKey();
    }

    private async Task ListGameTopicsAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> List game topics");
        var gameId = Ui.ReadGuid("Enter GameId");
        Ui.Write();
        var topics = await _gameService.GetTopicsAsync(gameId);
        Ui.Write(FormatTopic("Name", "Difficulty", "Lang"));
        foreach (var topic in topics)
        {
            Ui.Write("------------------------------------------------------------------------");
            Ui.Write(FormatTopic(topic.Name, topic.Difficulty.ToString(), topic.Lang));
            Ui.Write();
            Ui.Write("Questions of the topic:");
            Ui.Write(FormatQuestion("Type", "Difficulty", "Text"));
            foreach (var question in topic.Questions)
            {
                Ui.Write(FormatQuestion(question.Type.ToString(), question.Difficulty.ToString(), question.Text));
            }
            Ui.Write();
        }
        Ui.Write();
        Ui.PressKey();
    }
    private async Task ListGameFinalTopicsAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> List game final topics");
        var gameId = Ui.ReadGuid("Enter GameId");
        Ui.Write();
        var topics = await _gameService.GetTopicsFinalAsync(gameId);
        Ui.Write(FormatTopic("Name", "Difficulty", "Lang"));
        foreach (var topic in topics)
        {
            Ui.Write("------------------------------------------------------------------------");
            Ui.Write(FormatTopic(topic.Name, topic.Difficulty.ToString(), topic.Lang));
            Ui.Write();
            Ui.Write("Questions of the topic:");
            Ui.Write(FormatQuestion("Type", "Difficulty", "Text"));
            foreach (var question in topic.Questions)
            {
                Ui.Write(FormatQuestion(question.Type.ToString(), question.Difficulty.ToString(), question.Text));
            }
            Ui.Write();
        }
        Ui.Write();
        Ui.PressKey();
    }

    private async Task GameGetCatInBagAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> List game final topics");
        var gameId = Ui.ReadGuid("Enter GameId");
        Ui.Write();
        var topic = await _gameService.GetCatQuestionAsync(gameId);
        Ui.Write(FormatTopic("Name", "Difficulty", "Lang"));
        Ui.Write(FormatTopic(topic.Name, topic.Difficulty.ToString(), topic.Lang));
        Ui.Write("Questions of the topic:");
        Ui.Write(FormatQuestion("Type", "Difficulty", "Text"));
        foreach (var question in topic.Questions)
        {
            Ui.Write(FormatQuestion(question.Type.ToString(), question.Difficulty.ToString(), question.Text));
        }
        Ui.Write();
        Ui.PressKey();
    }
}