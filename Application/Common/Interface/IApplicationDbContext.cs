using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interface;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}
