using DonkeyLive.Shared.Extensions;
using DonkeyLive.WebApi.Handlers;
using DonkeyLive.WebApi.Helpers;
using DonkeyLive.WebApi.Services;
using FFMpegCore;
using LiveStreamingServerNet;
using LiveStreamingServerNet.AdminPanelUI;
using LiveStreamingServerNet.Flv.Installer;
using LiveStreamingServerNet.Networking.Contracts;
using LiveStreamingServerNet.Networking.Helpers;
using LiveStreamingServerNet.Standalone;
using LiveStreamingServerNet.Standalone.Insatller;
using Microsoft.AspNetCore.Hosting.Server;
using System.Diagnostics;
using System.Net;

namespace DonkeyLive.WebApi.Setups;

public static class RtmpServerSetup
{
    private static ILiveStreamingServer liveStreamingServer = null!;
    public static ILiveStreamingServer LiveStreamingServer => liveStreamingServer;

    public static LiveClientManager ClientManager { get; } = new LiveClientManager();

    public static string Base { get; private set; } = string.Empty;

    public static void AddRtmpServer(this IServiceCollection services, IConfiguration configuration)
    {
        liveStreamingServer = LiveStreamingServerBuilder.Create()
        .ConfigureRtmpServer(options =>
        {
            options.AddStandaloneServices().AddFlv();
            options.AddStreamEventHandler<RtmpServerHandler>();
        })
        .ConfigureLogging(options => options.AddConsole())
        .Build();

        var rtmpPort = configuration.GetSection("Application:RtmpPort").Get<int>();
        if (rtmpPort == 0)
        {
            rtmpPort = 1935;
        }

        services.AddBackgroundServer(liveStreamingServer, new IPEndPoint(IPAddress.Any, rtmpPort));
    }

    public static void UseRtmpServer(this WebApplication app)
    {
        Debug.Assert(liveStreamingServer.IsNotNull(), "liveStreamingServer is not null");

        //app.UseWebSockets();
        //app.UseWebSocketFlv(liveStreamingServer);
        app.UseHttpFlv(liveStreamingServer);

        app.MapStandaloneServerApiEndPoints(liveStreamingServer);
        app.UseAdminPanelUI(new AdminPanelUIOptions { BasePath = "/ui", HasHttpFlvPreview = true });

        Base = $"http://{app.Configuration.GetSection("Application:Host").Value}:{app.Configuration.GetSection("Application:Port").Value}";

        var webEnv = app.Services.GetRequiredService<IWebHostEnvironment>();
        WWWRoot = Path.Combine(webEnv.ContentRootPath, "wwwroot");
    }

    public static string WWWRoot { get; private set; } = string.Empty;

    public static void Snap(string streamPath)
    {
        Task.Run(async () =>
        {
            await Task.Delay(5000); // 等待 5 秒

            var streamId = streamPath.TrimEnd('/').Split('/').Last();
            var outputPath = Path.Combine(WWWRoot, "covers", $"{streamId}.jpg");

            try
            {
                await FFMpegArguments
                    .FromUrlInput(new Uri($"{Base.TrimEnd('/')}/live/{streamId}.flv"))
                    .OutputToFile(outputPath, true, options => options
                        .Seek(TimeSpan.FromSeconds(5)) // 从流开始后的 5 秒处截取
                        .WithVideoCodec("mjpeg") // 设置输出格式为 JPG
                        .WithFrameOutputCount(1)) // 仅输出一个帧（即一张图片）
                    .ProcessAsynchronously();
            }
            catch (Exception ex)
            {
                LogHelper.LogError<FFMpegArguments>($"Snap error: {streamPath} || {outputPath}", ex);
            }
        });
    }
}
