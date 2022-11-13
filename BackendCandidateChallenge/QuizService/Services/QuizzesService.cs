using Dapper;
using Microsoft.Extensions.Logging;
using QuizService.Mappings;
using QuizService.Model;
using QuizService.Model.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuizService.Services;

// QuizService is named as QuizzesService, due to conflict with the namespace.
public class QuizzesService : IQuizzesService
{
    private readonly IDbConnection _connection;
    private readonly ILogger<QuizzesService> _logger;

    public QuizzesService(IDbConnection connection, ILogger<QuizzesService> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public QuizResponseModel GetById(int id)
    {
        try
        {
            _logger.LogInformation($"Fetching quiz. Id: {id}");
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

            if (quiz == null)
            {
                _logger.LogInformation($"Quiz not found. Id: {id}");
                return null;
            }

            return quiz.ToDetailedQuizResponseModel(questions, id, answers);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured while fetching quiz", ex);
            throw;
        }
    }

    public IEnumerable<QuizResponseModel> GetAll()
    {
        try
        {
            _logger.LogInformation("Fetching quiz list");
            const string sql = "SELECT Id, Title FROM Quiz;";
            var quizzes = _connection.Query<Quiz>(sql);

            return quizzes.Select(quiz => quiz.ToBasicQuizResponseModel());
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured while fetching quizzes", ex);
            throw;
        }
    }
}

public interface IQuizzesService
{
    public IEnumerable<QuizResponseModel> GetAll();
    public QuizResponseModel GetById(int id);
}