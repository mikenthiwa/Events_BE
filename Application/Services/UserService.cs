using Application.Common.Interface;
using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService(IApplicationDbContext applicationDbContext) : IUserService
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
    public async Task<User> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };
        _applicationDbContext.Users.Add(user);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
    
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Users.AnyAsync(user => user.Email == email, cancellationToken)
            ? await _applicationDbContext.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken)
            : null;
    }
}
