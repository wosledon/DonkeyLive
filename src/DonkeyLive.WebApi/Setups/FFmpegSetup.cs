using FFMpegCore;

namespace DonkeyLive.WebApi.Setups;

public static class FFmpegSetup
{
    public static void AddFFmpeg(this IServiceCollection services, IConfiguration configuration)
    {
        if (OperatingSystem.IsWindows())
        {
            GlobalFFOptions.Configure(opts =>
            {
                opts.BinaryFolder = Path.Combine("D:/ffmpeg/bin");
            });
        }
    }
}
