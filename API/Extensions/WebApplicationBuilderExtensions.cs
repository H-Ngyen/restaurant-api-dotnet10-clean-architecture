using Serilog;

namespace API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Information)
                .WriteTo.Console(
                    // debug template
                    // outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] | {SourceContext} | {NewLine}{Message:lj}{NewLine}{Exception}"
                    outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] | {Message:lj}{NewLine}{Exception}"
                )
        );
    }
}