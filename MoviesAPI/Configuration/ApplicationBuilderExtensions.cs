using MoviesAPI.Middleware;

namespace MoviesAPI.Configuration;

/// <summary>
/// Extension methods for configuring the HTTP request pipeline
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configure middleware pipeline for development environment
    /// </summary>
    public static IApplicationBuilder UseDevelopmentConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    /// <summary>
    /// Configure error handling middleware
    /// </summary>
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseErrorHandling();
        return app;
    }

    /// <summary>
    /// Configure standard middleware pipeline
    /// </summary>
    public static IApplicationBuilder UseStandardMiddleware(this IApplicationBuilder app)
    {
        app.UseCors("AllowAll");
        app.UseResponseCaching();
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
