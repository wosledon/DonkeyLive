using DonkeyLive.Shared.Domain.Base;

namespace DonkeyLive.Shared.Domain;

public class LiveGroup : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public virtual ICollection<LiveTag> Tags { get; set; } = new List<LiveTag>();
}
