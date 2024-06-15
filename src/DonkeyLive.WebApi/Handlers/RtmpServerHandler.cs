using DonkeyLive.WebApi.Helpers;
using DonkeyLive.WebApi.Setups;
using FFMpegCore;
using LiveStreamingServerNet.Rtmp;
using LiveStreamingServerNet.Rtmp.Contracts;
using LiveStreamingServerNet.Utilities.Contracts;

namespace DonkeyLive.WebApi.Handlers;

public class RtmpServerHandler : IRtmpServerStreamEventHandler
{
    public ValueTask OnRtmpStreamPublishedAsync(IEventContext context, uint clientId, string streamPath, IReadOnlyDictionary<string, string> streamArguments)
    {
        RtmpServerSetup.ClientManager.AddOrUpdate(clientId, streamPath);
        RtmpServerSetup.Snap(streamPath);

        return ValueTask.CompletedTask;
    }

    public ValueTask OnRtmpStreamUnpublishedAsync(IEventContext context, uint clientId, string streamPath)
    {
        RtmpServerSetup.ClientManager.Remove(clientId);

        return ValueTask.CompletedTask;
    }


    public ValueTask OnRtmpStreamMetaDataReceivedAsync(
        IEventContext context, uint clientId, string streamPath, IReadOnlyDictionary<string, object> metaData)
        => ValueTask.CompletedTask;

    public ValueTask OnRtmpStreamSubscribedAsync(
        IEventContext context, uint clientId, string streamPath, IReadOnlyDictionary<string, string> streamArguments)
        => ValueTask.CompletedTask;

    public ValueTask OnRtmpStreamUnsubscribedAsync(IEventContext context, uint clientId, string streamPath)
        => ValueTask.CompletedTask;
}