using DonkeyLive.Shared.RequestInput.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyLive.Shared.RequestInput;

public class LiveRoomRequestInput : IPaging, IRequestInput
{
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 15;
}
