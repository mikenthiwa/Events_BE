using Microsoft.EntityFrameworkCore;

namespace Application.Model;

public class PaginatedList<T>(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
{
    public int PageNumber { get; } = pageNumber;
    public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
    public int TotalCount { get; } = count;
    public IReadOnlyCollection<T> Items { get; } = items;
    
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
      var count = await source.CountAsync(cancellationToken);
      var items = await source.Skip((pageNumber - 1) * pageSize).ToListAsync(cancellationToken);
      return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

}
