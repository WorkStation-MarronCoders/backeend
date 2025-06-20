using System;
using workstation_backend.UserContext.Domain.Models.Entities;
using workstation_backend.Shared.Domain.Repositories;

namespace workstation_backend.UserContext.Domain;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IEnumerable<User>> GetAllAsync();

    Task<User?> GetByDniAsync(string dni);
}
