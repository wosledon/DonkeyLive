using DonkeyLive.Shared.Helpers;
using DonkeyLive.Shared.RequestInput.Base;
using DonkeyLive.WebApi.Helpers;
using System.Linq.Expressions;

namespace DonkeyLive.WebApi.Extensions;

public static class QueryableExtensions
{
    public static async Task<Helpers.PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, IRequestInput input)
        where T:class
    {
        if(input is IPaging page)
        {
            return await Helpers.PagedList<T>.ToPagedListAsync(query, page.PageIndex, page.PageSize);
        }

        return await Helpers.PagedList<T>.ToPagedListAsync(query, 1, 999);
    }

    public static Helpers.PagedList<T> ToPagedList<T>(this IQueryable<T> query, IRequestInput input)
        where T : class
    {
        if(input is IPaging page)
        {
            return Helpers.PagedList<T>.ToPagedList(query, page.PageIndex, page.PageSize);
        }

        return Helpers.PagedList<T>.ToPagedList(query, 1, 999);
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IRequestInput input)
    {
        if(input is IPaging page)
        {
            return query.Skip((page.PageIndex - 1) * page.PageSize).Take(page.PageSize);
        }

        return query;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool @if, Expression<Func<T, bool>> exp)
    {
        if(@if)
        {
            return query.Where(exp);
        }
        return query;
    }

    public static IQueryable<T> WhereNotIf<T>(this IQueryable<T> query, bool @if, Expression<Func<T, bool>> exp)
    {
        return query.WhereIf(!@if, exp);
    }
}
