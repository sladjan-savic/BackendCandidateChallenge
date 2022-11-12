using Dapper;
using QuizService.Mappings;
using QuizService.Model;
using QuizService.Model.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuizService.Services;

// QuizService is named as QuizzesService, due to conflict with the namespace.
public class QuizzesService : IQuizzesService
{
    private readonly IDbConnection _connection;

    public QuizzesService(IDbConnection connection)
    {
        _connection = connection;
    }

    public QuizResponseModel GetById(int id)
    {
        const string quizSql = "SELECT Id, Title FROM Quiz WHERE Id = @Id;";
        var quiz = _connection.QueryFirstOrDefault<Quiz>(quizSql, new { Id = id });

        const string questionsSql = "SELECT Id, QuizId, Text, CorrectAnswerId FROM Question WHERE QuizId = @QuizId;";
        var questions = _connection.Query<Question>(questionsSql, new { QuizId = id });

        const string answersSql = "SELECT a.Id, a.Text, a.QuestionId FROM Answer a INNER JOIN Question q ON a.QuestionId = q.Id WHERE q.QuizId = @QuizId;";
        var answers = _connection.Query<Answer>(answersSql, new { QuizId = id })
            .Aggregate(new Dictionary<int, IList<Answer>>(), (dict, answer) =>
            {
                if (!dict.ContainsKey(answer.QuestionId))
                    dict.Add(answer.QuestionId, new List<Answer>());
                dict[answer.QuestionId].Add(answer);
                return dict;
            });

        if (quiz == null) return null;

        return quiz.ToDetailedQuizResponseModel(questions, id, answers);
    }

    public IEnumerable<QuizResponseModel> GetAll()
    {
        const string sql = "SELECT Id, Title FROM Quiz;";
        var quizzes = _connection.Query<Quiz>(sql);

        return quizzes.Select(quiz => quiz.ToBasicQuizResponseModel());
    }
}

public interface IQuizzesService
{
    public IEnumerable<QuizResponseModel> GetAll();
    public QuizResponseModel GetById(int id);
}