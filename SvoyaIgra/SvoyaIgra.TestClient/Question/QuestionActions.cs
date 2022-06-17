using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.TestClient.Question;

public class QuestionActions: IQuestionActions
{
    private readonly IQuestionService _questionService;

    public QuestionActions(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    public void PerformQuestionAction()
    {
        throw new NotImplementedException();
    }
}