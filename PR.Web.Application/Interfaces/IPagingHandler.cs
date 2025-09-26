using PR.Web.Application.Core;

namespace PR.Web.Application.Interfaces;

public interface IPagingHandler<T>
{
    Task<PagedList<T>> CreateAsync(
         IQueryable<T> source,
         int pageNumber,
         int pageSize);

    PagedList<T> Create(
        IEnumerable<T> source,
        int pageNumber,
        int pageSize);
}
