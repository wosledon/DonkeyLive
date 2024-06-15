namespace DonkeyLive.Shared.Domain.Base;

public interface ISoftDelete
{
    public bool Deleted { get; set; }
}
