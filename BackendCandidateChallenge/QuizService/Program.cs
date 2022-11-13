using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace QuizService;

public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(l =>
            {
                // Sending logs to the console for testing purposes.
                l.ClearProviders(); 
                l.AddConsole();
                l.SetMinimumLevel(LogLevel.Debug);
            })
            .UseStartup<Startup>()
            .Build();
}