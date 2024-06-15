namespace DonkeyLive.Front.Services.Base;

public class UrlMap
{
    public class LiveRoom
    {
        private const string Base = "api/v1/liveroom";

        public const string GetLiveRooms = $"{Base}/list";
        public const string GetLiveRoom = $"{Base}/get";
        public const string CreateLiveRoom = $"{Base}/post";
        public const string UpdateLiveRoom = $"{Base}/put";
        public const string DeleteLiveRoom = $"{Base}/delete";
        public const string GetLive = $"{Base}/getlive";
    }
}
