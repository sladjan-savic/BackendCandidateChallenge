using Dapper;
using Microsoft.AspNetCore.Mvc;
using QuizService.Model;
using QuizService.Services;
using System.Collections.Generic;
using System.Data;

namespace QuizService.Controllers;

[Route("api/quizzes")]
public class QuizController : Controller
{
    private readonly IDbConnection _connection;
    private readonly IQuizzesService _quizesService;

    public QuizController(IDbConnection connection, IQuizzesService quizesService)
    {
        _connection = connection;
        _quizesService = quizesService;
    }

    // TODO: In my opinion, Entity Framework would be much better choice for the ORM here.
    // The only advantage of Dapper over EF is it's performance,
    // useful in case of some really complex or taxing query, which is not the case in this lightweigt application.
    // On the other hand, EF has Unit-of-work and repository patterns implemented (DbContext, DbSets),
    // supports migration management, linq-to-sql, intuitive entity configuration.

    // GET api/quizzes
    [HttpGet]
    public ActionResult<IEnumerable<QuizResponseModel>> Get()
    {
        var result = _quizesService.GetAll();

        return Ok(result);
    }

    // GET api/quizzes/5
    [HttpGet("{id}")]
    public ActionResult<QuizResponseModel> Get(int id)
    {
        var quiz = _quizesService.GetById(id);

        if (quiz == null)
            return NotFound();

        return Ok(quiz);
    }

    // POST api/quizzes
    [HttpPost]
    public IActionResult Post([FromBody] QuizCreateModel value)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Service methods are reusable within solution.
        // TODO: Validate QuizCreateModel. Possible solution is FluentValidation.
        // TODO: Wrap this logic with try-catch. Add logs.

        var sql = $"INSERT INTO Quiz (Title) VALUES('{value.Title}'); SELECT LAST_INSERT_ROWID();";
        var id = _connection.ExecuteScalar(sql);
        return Created($"/api/quizzes/{id}", null);
    }

    // PUT api/quizzes/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] QuizUpdateModel value)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Service methods are reusable within solution.
        // TODO: Validate QuizUpdateModel. Possible solution is FluentValidation.
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "UPDATE Quiz SET Title = @Title WHERE Id = @Id";
        int rowsUpdated = _connection.Execute(sql, new { Id = id, Title = value.Title });
        if (rowsUpdated == 0)
            return NotFound();
        return NoContent();
    }

    // DELETE api/quizzes/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Service methods are reusable within solution.
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "DELETE FROM Quiz WHERE Id = @Id";
        int rowsDeleted = _connection.Execute(sql, new { Id = id });
        if (rowsDeleted == 0)
            return NotFound();
        return NoContent();
    }

    // POST api/quizzes/5/questions
    [HttpPost]
    [Route("{id}/questions")]
    public IActionResult PostQuestion(int id, [FromBody] QuestionCreateModel value)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Service methods are reusable within solution.
        // TODO: Validate QuizCreateModel. Possible solution is FluentValidation.
        // Since we are referencing the quiz entity, do check if it exists.
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "INSERT INTO Question (Text, QuizId) VALUES(@Text, @QuizId); SELECT LAST_INSERT_ROWID();";
        var questionId = _connection.ExecuteScalar(sql, new { Text = value.Text, QuizId = id });
        return Created($"/api/quizzes/{id}/questions/{questionId}", null);
    }

    // PUT api/quizzes/5/questions/6
    [HttpPut("{id}/questions/{qid}")]
    public IActionResult PutQuestion(int id, int qid, [FromBody] QuestionUpdateModel value)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Service methods are reusable within solution.
        // Either remove unused parameter of find usage for it (check if related data exists perhaps).
        // Possible solutions is to move the endpoint into another controller: QuestionsController
        // TODO: Validate QuestionUpdateModel. Possible solution is FluentValidation.
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "UPDATE Question SET Text = @Text, CorrectAnswerId = @CorrectAnswerId WHERE Id = @QuestionId";
        int rowsUpdated = _connection.Execute(sql, new { QuestionId = qid, Text = value.Text, CorrectAnswerId = value.CorrectAnswerId });
        if (rowsUpdated == 0)
            return NotFound();
        return NoContent();
    }

    // DELETE api/quizzes/5/questions/6
    [HttpDelete]
    [Route("{id}/questions/{qid}")]
    public IActionResult DeleteQuestion(int id, int qid)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Service methods are reusable within solution.
        // Either remove unused parameter or find usage for it.
        // Possible solutions is to move the endpoint into another controller: QuestionsController
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "DELETE FROM Question WHERE Id = @QuestionId";
        _connection.ExecuteScalar(sql, new { QuestionId = qid });
        return NoContent();
    }

    // POST api/quizzes/5/questions/6/answers
    [HttpPost]
    [Route("{id}/questions/{qid}/answers")]
    public IActionResult PostAnswer(int id, int qid, [FromBody] AnswerCreateModel value)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Service methods are reusable within solution.
        // TODO: Validate AnswerCreateModel. Possible solution is FluentValidation.
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "INSERT INTO Answer (Text, QuestionId) VALUES(@Text, @QuestionId); SELECT LAST_INSERT_ROWID();";
        var answerId = _connection.ExecuteScalar(sql, new { Text = value.Text, QuestionId = qid });
        return Created($"/api/quizzes/{id}/questions/{qid}/answers/{answerId}", null);
    }

    // PUT api/quizzes/5/questions/6/answers/7
    [HttpPut("{id}/questions/{qid}/answers/{aid}")]
    public IActionResult PutAnswer(int id, int qid, int aid, [FromBody] AnswerUpdateModel value)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Also, service methods are reusable within solution.
        // Either remove unused parameters or use them.
        // TODO: Validate AnswerUpdateModel. Possible solution is FluentValidation.
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "UPDATE Answer SET Text = @Text WHERE Id = @AnswerId";

        // TODO: the next line reads: ... new {AnswerId = qid... Is this correct?
        int rowsUpdated = _connection.Execute(sql, new { AnswerId = qid, Text = value.Text });
        if (rowsUpdated == 0)
            return NotFound();
        return NoContent();
    }

    // DELETE api/quizzes/5/questions/6/answers/7
    [HttpDelete]
    [Route("{id}/questions/{qid}/answers/{aid}")]
    public IActionResult DeleteAnswer(int id, int qid, int aid)
    {
        // TODO: Controllers should not contain any logic. Move this code into dedicated service method.
        // Also, service methods are reusable within solution.
        // Either remove unused parameters or use them. Possible solutions is to move the endpoint into another controller: AnswersController.
        // TODO: Wrap this logic with try-catch. Add logs.

        const string sql = "DELETE FROM Answer WHERE Id = @AnswerId";
        _connection.ExecuteScalar(sql, new { AnswerId = aid });
        return NoContent();
    }
}