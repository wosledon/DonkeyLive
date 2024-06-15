using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyLive.Shared.RequestInput.Base;

public interface IPaging
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public interface IRequestInput
{

}
