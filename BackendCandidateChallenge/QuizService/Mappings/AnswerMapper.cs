using QuizService.Model.Domain;

using static QuizService.Model.QuizResponseModel;

namespace QuizService.Mappings;

public static class AnswerMapper
{
    public static AnswerItem ToAnswerItem(this Answer answer)
    {
        if (answer == null) return null;

        return new AnswerItem
        {
            Id= answer.Id,
            Text= answer.Text,
        };
    }
}
