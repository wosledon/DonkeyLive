using DonkeyLive.Front.Services.Base;
using DonkeyLive.Front.Services.Global;
using DonkeyLive.Shared.Domain;
using DonkeyLive.Shared.Helpers;
using DonkeyLive.Shared.RequestInput;

namespace DonkeyLive.Front.Services;

public class LiveRoomService: IHttpService
{
    private readonly HttpRequestService _httpRequestService;
    private readonly AlertService _alert;

    public LiveRoomService(HttpRequestService httpRequestService
        , AlertService alert)
    {
        _httpRequestService = httpRequestService;
        _alert = alert;
    }

    public async Task<PagedList<LiveRoom>> GetLiveRoomsAsync(LiveRoomRequestInput input)
    {
        var res = await _httpRequestService
            .GetAsync<ApiResponse<PagedList<LiveRoom>>>(UrlMap.LiveRoom.GetLiveRooms, input);

        return await _alert.ProcessResultAsync(res, [], false);
    }

    public async Task<LiveRoom> GetLiveRoomAsync(Guid id)
    {
        var res = await _httpRequestService
            .GetAsync<ApiResponse<LiveRoom>>(UrlMap.LiveRoom.GetLiveRoom, new { id });

        return await _alert.ProcessResultAsync(res, new LiveRoom());
    }

    public async Task CreateLiveRoomAsync(LiveRoom room)
    {
        var res = await _httpRequestService
            .PostAsync<ApiResponse<LiveRoom>>(UrlMap.LiveRoom.CreateLiveRoom, null, room);

        await _alert.ProcessResultAsync(res, new LiveRoom());
    }

    public async Task UpdateLiveRoomAsync(LiveRoom room)
    {
        var res = await _httpRequestService
            .PutAsync<ApiResponse<object>>(UrlMap.LiveRoom.UpdateLiveRoom, new { room.Id }, room);

        await _alert.ProcessResultAsync(res, false);
    }

    public async Task DeleteLiveRoomAsync(Guid id)
    {
        var res = await _httpRequestService
            .DeleteAsync<ApiResponse<object>>(UrlMap.LiveRoom.DeleteLiveRoom, new {id});

        await _alert.ProcessResultAsync(res, false);
    }

    public async Task<string> GetLiveAsync(Guid id)
    {
        var res = await _httpRequestService
            .GetAsync<ApiResponse<string>>(UrlMap.LiveRoom.GetLive, new { id });

        return await _alert.ProcessResultAsync(res, "");
    }
}
