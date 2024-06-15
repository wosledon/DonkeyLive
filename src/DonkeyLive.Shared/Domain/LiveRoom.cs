using DonkeyLive.Shared.Domain.Base;
using DonkeyLive.Shared.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonkeyLive.Shared.Domain;

public class LiveRoom : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime PlayAt { get; set; }

    public Guid? LiveTagId { get; set; }
    public virtual LiveTag? LiveTag { get; set; }

    [NotMapped]
    public string RoomId => Id.ToString();
    [NotMapped]
    public bool IsOnline { get; set; }
    [NotMapped]
    public string Cover => $"{RoomId}.jpg";
}
