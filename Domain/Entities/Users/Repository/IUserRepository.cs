using Domain.Entities.Users.Models;
using Domain.Shared.Abstractions;

namespace Domain.Entities.Users.Repository;

public interface IUserRepository : IRepository<User>
{
}
