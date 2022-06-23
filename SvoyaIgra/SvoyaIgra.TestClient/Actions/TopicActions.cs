using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.TestClient.Actions;

public class TopicActions : ITopicActions
{
    private readonly ITopicService _topicService;
    private readonly IQuestionService _questionService;

    public TopicActions(ITopicService topicService, IQuestionService questionService)
    {
        _topicService = topicService;
        _questionService = questionService;
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
                    //SetUserStatusAsync(true).Wait();
                    break;
                case 5:
                    //ResetPasswordAsync().Wait();
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
        Ui.Write(" 4. ");
        Ui.Write(" 5. ");
        Ui.Write(" 0. <- BACK");
        return Ui.Choice(5);
    }

    private async Task ListTopicsAsync()
    {
        Ui.Clear();
        Ui.Write("Topic -> List all topics");
        var topics = await _topicService.GetAllTopicsAsync();
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
        Ui.Write(FormatTopic("Name", "Difficulty"));
        Ui.Write(FormatTopic(topic.Name, topic.Difficulty.ToString()));
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
        var allowedValues = ((TopicDifficulty[])Enum.GetValues(typeof(TopicDifficulty))).Select(c => (int)c).ToArray();
        var difficulty = Ui.ReadInt("Difficulty", allowedValues);

        var resp = await _topicService.CreateTopicAsync(name, (TopicDifficulty)difficulty);
        if (resp == null)
        {
            return;
        }
        Ui.Write();
        Ui.Write("Topic successfully created");
        Ui.Write($"Name: {resp.Name}\t\tDifficulty: {resp.Difficulty}");
        Ui.PressKey();
    }
}