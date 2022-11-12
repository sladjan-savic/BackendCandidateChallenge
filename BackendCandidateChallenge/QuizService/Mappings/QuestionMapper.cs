using QuizService.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using static QuizService.Model.QuizResponseModel;

namespace QuizService.Mappings;

public static class QuestionMapper
{
    public static QuestionItem ToQuestionItem(this Question question, Dictionary<int, IList<Answer>> answers)
    {
        if (question == null) return null;

        return new QuestionItem
        {
            Id = question.Id,
            Text = question.Text,
            Answers = answers.ContainsKey(question.Id)
                    ? answers[question.Id].Select(answer => answer.ToAnswerItem())
                    : Array.Empty<AnswerItem>(),
            CorrectAnswerId = question.CorrectAnswerId
        };
    }
}
