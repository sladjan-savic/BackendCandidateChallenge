using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizService.Services;

namespace QuizService.Extensions
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration _)
        {
            services.AddScoped<IQuizzesService, QuizzesService>();
        }
    }
}
