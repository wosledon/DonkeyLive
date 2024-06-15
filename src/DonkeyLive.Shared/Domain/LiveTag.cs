using DonkeyLive.Shared.Domain.Base;

namespace DonkeyLive.Shared.Domain;

public class LiveTag : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public virtual ICollection<LiveRoom> Rooms { get; set; } = new List<LiveRoom>();
}