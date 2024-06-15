using System.Diagnostics;

namespace DonkeyLive.WebApi.Helpers;

public static class LogHelper
{
    private static IApplicationBuilder _app = null!;

    private static ILogger<T> GetLogger<T>()
    {
        Debug.Assert(_app != null, "Please call UseLogger first");

        return _app.ApplicationServices.GetRequiredService<ILogger<T>>();
    }

    public static void UseLogger(this IApplicationBuilder app)
    {
        _app = app;
    }

    public static void LogInformation<T>(string message, Exception? ex)
    {
        GetLogger<T>().LogInformation(ex, message);
    }

    public static void LogError<T>(string message, Exception? ex)
    {
        GetLogger<T>().LogError(ex, message);
    }

    public static void LogWarning<T>(string message, Exception? ex)
    {
        GetLogger<T>().LogWarning(ex, message);
    }

    public static void LogDebug<T>(string message, Exception? ex)
    {
        GetLogger<T>().LogDebug(ex, message);
    }
}
