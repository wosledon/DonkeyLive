using DonkeyLive.Shared.Domain;
using DonkeyLive.Shared.Extensions;
using DonkeyLive.Shared.RequestInput;
using DonkeyLive.WebApi.Controllers.Base;
using DonkeyLive.WebApi.Extensions;
using DonkeyLive.WebApi.Options;
using DonkeyLive.WebApi.Services;
using DonkeyLive.WebApi.Setups;
using LiveStreamingServerNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace DonkeyLive.WebApi.Controllers;

public class LiveRoomController : ApiControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IOptions<AppSettings> _appSettings;

    public LiveRoomController(UnitOfWork unitOfWork
        , IOptions<AppSettings> appSettings)
    {
        _unitOfWork = unitOfWork;
        _appSettings = appSettings;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery]LiveRoomRequestInput input)
    {
        var rooms = await _unitOfWork.Query<LiveRoom>()
            .WhereIf(input.Keyword.IsNotEmpty(), x=>x.Name.Contains(input.Keyword!) 
                || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.Contains(input.Keyword!)))
            .OrderByDescending(x=>x.PlayAt)
            .ToPagedListAsync(input);

        rooms.ForEach(x => x.IsOnline = RtmpServerSetup.ClientManager.IsOnline(x.RoomId));

        return Success(rooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var room = await _unitOfWork.Query<LiveRoom>().FirstOrDefaultAsync(x => x.Id == id);
        if (room == null)
        {
            return Error("Room not found");
        }

        return Success(room);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LiveRoom room)
    {
        if (room.IsNull())
        {
            return Error("room is null");
        }

        //if (room.Id.IsNotEmpty())
        //{
        //    return Error("room id must be empty");
        //}

        _unitOfWork.Add(room);
        await _unitOfWork.SaveChangesAsync();
        return Success("Room created", room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] LiveRoom room)
    {
        var existingRoom = _unitOfWork.Query<LiveRoom>().FirstOrDefault(x => x.Id == id);
        if (existingRoom == null)
        {
            return Error("Room not found");
        }

        existingRoom.Name = room.Name;
        existingRoom.Description = room.Description;
        existingRoom.PlayAt = room.PlayAt;

        await _unitOfWork.SaveChangesAsync();
        return Success("Room updated", existingRoom);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingRoom = _unitOfWork.Query<LiveRoom>().FirstOrDefault(x => x.Id == id);
        if (existingRoom == null)
        {
            return Error("Room not found");
        }

        _unitOfWork.Delete(existingRoom);
        await _unitOfWork.SaveChangesAsync();
        return Success("Room deleted");
    }

    [HttpGet]
    public async Task<IActionResult> GetLiveAsync(Guid id)
    {

        var room = await _unitOfWork.Query<LiveRoom>().FirstOrDefaultAsync(x => x.Id == id);
        if (room == null)
        {
            return Error("Room not found");
        }

        return Success("",$"{_appSettings.Value.RtmpBase}/{room.RoomId}");
    }
}