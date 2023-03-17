using Auth.Domain;

namespace Auth.Application.Ports.Repositories;

public interface IAuthRepository
{
    Task<User> GetUserByEmail(string email);
    Task UpdateUser(User user);
    Task<User> GetUserByUserId(Guid userId);
    Task CreateUser(User user);
}