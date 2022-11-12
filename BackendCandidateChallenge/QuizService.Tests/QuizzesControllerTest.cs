using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using QuizService.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace QuizService.Tests;

public class QuizzesControllerTest
{
    const string QuizApiEndPoint = "/api/quizzes/";

    [Fact]
    public async Task PostNewQuizAddsQuiz()
    {
        var quiz = new QuizCreateModel("Test title");
        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(quiz));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}"),
                content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }
    }

    // TODO: Turn the test case into Theory and parametrize it, instead of hardcoding any values (quizId).
    [Fact]
    public async Task AQuizExistGetReturnsQuiz()
    {
        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            // TODO: Unit tests should not make multiple asserts, unless we are testing the state of a single object.
            // Break up this test case into smaller tests.
            var client = testHost.CreateClient();
            const long quizId = 1;
            var response = await client.GetAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}{quizId}"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            var quiz = JsonConvert.DeserializeObject<QuizResponseModel>(await response.Content.ReadAsStringAsync());
            Assert.Equal(quizId, quiz.Id);
            Assert.Equal("My first quiz", quiz.Title);
        }
    }

    // TODO: Turn the test case into Theory and parametrize it, instead of hardcoding any values (quizId).
    [Fact]
    public async Task AQuizDoesNotExistGetFails()
    {
        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            const long quizId = 999;
            var response = await client.GetAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}{quizId}"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    // TODO: Turn the test case into Theory and parametrize it, instead of hardcoding any values (quizId).
    [Fact]

    public async Task AQuizDoesNotExists_WhenPostingAQuestion_ReturnsNotFound()
    {
        // TODO: QuizApiEndPoint is using hardcoded quizId value. Interpolate quizId into variable string.
        const string QuizApiEndPoint = "/api/quizzes/999/questions";

        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            const long quizId = 999;
            var question = new QuestionCreateModel("The answer to everything is what?");
            var content = new StringContent(JsonConvert.SerializeObject(question));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}{quizId}"), content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    [Theory]
    [InlineData(3)]
    public async Task AQuizHasCorrectAnswersGetReturnsQuiz(long quizId)
    {
        // Test would make much more sense if Correct answer was textual instead of id reference field:
        // var correctAnswers = quiz.Questions.Select(x => x.CorrectAnswerText.ToLower()).ToList();
        // var correctlyAnswered = quiz.Questions.Select(x => x.Answers.Where(y => correctAnswers.Contains(y.Text.ToLower()))).Count();
        // Or something
        using var testHost = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        
        var client = testHost.CreateClient();
        var response = await client.GetAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}{quizId}"));

        var quiz = JsonConvert.DeserializeObject<QuizResponseModel>(await response.Content.ReadAsStringAsync());
        var correctAnswerIds = quiz.Questions.Select(x => x.CorrectAnswerId).ToList();

        var correctlyAnswered = quiz.Questions.Select(x => x.Answers.Where(y => correctAnswerIds.Contains(y.Id))).Count();

        Assert.Equal(quizId, quiz.Id);
        Assert.Equal("My third quiz", quiz.Title);
        Assert.Equal(2, correctlyAnswered);
    }
}
