using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.TestClient.Question;

public class QuestionActions: IQuestionActions
{
    private readonly IQuestionService _themeService;

    public QuestionActions(IQuestionService questionService)
    {
        _themeService = questionService;
    }

    public void PerformQuestionAction()
    {
        throw new NotImplementedException();
    }
}