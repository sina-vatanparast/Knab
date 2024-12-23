using Knab.Cryptocurrency.Presentation.Extensions;
using Serilog;

namespace Knab.Cryptocurrency;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.RegisterLogger();

        builder.Services.RegisterServices(builder.Configuration);

        var app = builder.Build();

        app.ConfigurePipeline();

        try
        {
            Log.Information("Starting web host");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
