using Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Application.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<T>> PaginatedListAsync<T>(this IQueryable<T> queryable, int pageNumber,
        int pageSize, CancellationToken cancellationToken) where T : class
    {
        return PaginatedList<T>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);
    }
}
