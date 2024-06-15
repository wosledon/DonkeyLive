using Microsoft.Extensions.Options;

namespace DonkeyLive.WebApi.Options
{
    public class AppSettings : IOptions<AppSettings>
    {
        public int Port { get; set; } = 8080;
        public int RtmpPort { get; set; } = 1935;

        public string Host { get; set; } = "localhost";

        public string RtmpBase => $"rtmp://{Host}:{RtmpPort}/live";

        public string Base => $"http://{Host}:{Port}";

        public AppSettings Value => this;
    }
}
