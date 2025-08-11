using Domain.Entities;

namespace Application.Interface;

public interface IUserService
{
    Task<User> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}
