using DonkeyLive.Shared.Domain;
using System.Collections.Concurrent;

namespace DonkeyLive.WebApi.Services;

public class LiveClientManager
{
    private readonly static ConcurrentDictionary<uint, string> _clients = new();

    public bool IsOnline(string roomId)
    {
        return _clients.Values.Where(x => x.Contains(roomId)).Any();
    }

    public void AddOrUpdate(uint clientId, string roomId)
    {
        _clients.AddOrUpdate(clientId, roomId, (key, value) => roomId);
    }

    public void Remove(uint clientId)
    {
        _clients.TryRemove(clientId, out _);
    }
}
