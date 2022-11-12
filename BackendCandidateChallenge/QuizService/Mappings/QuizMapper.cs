using QuizService.Model;
using QuizService.Model.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QuizService.Mappings;

public static class QuizMapper
{
    public static QuizResponseModel ToBasicQuizResponseModel(this Quiz quiz)
    {
        if (quiz == null) return null;

        return new QuizResponseModel
        {
            Id = quiz.Id,
            Title = quiz.Title
        };
    }

    public static QuizResponseModel ToDetailedQuizResponseModel(this Quiz quiz, IEnumerable<Question> questions, long id, Dictionary<int, IList<Answer>> answers)
    {
        if (quiz == null) return null;

        return new QuizResponseModel
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Questions = questions.Select(question => question.ToQuestionItem(answers)),
            Links = new Dictionary<string, string>
        {
            {"self", $"/api/quizzes/{id}"},
            {"questions", $"/api/quizzes/{id}/questions"}
        }
        };
    }
}
