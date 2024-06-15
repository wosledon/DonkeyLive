using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyLive.Shared.Helpers;

public interface IPagedList
{
    public int TotalCount { get; set; }
}

public class PagedList<T>: List<T>, IPagedList where T:class
    {
    public int CurrentPage { get; private set; }

    public int TotalPages { get; private set; }

    public int PageSize { get; private set; }

    public int TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNex => CurrentPage < TotalPages;

    public PagedList()
    {
    }
}
