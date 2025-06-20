using System;
using Microsoft.EntityFrameworkCore;
using workstation_backend.UserContext.Domain;
using workstation_backend.UserContext.Domain.Models.Entities;
using workstation_backend.Shared.Infrastructure.Persistence.Configuration;
using workstation_backend.Shared.Infrastructure.Persistence.Repositories;
using workstation_backend.Shared.Domain;

namespace workstation_backend.UserContext.Infrastructure;

public class UserRepository(WorkstationContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByDniAsync(string dni)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(user => user.Dni == dni);
    }
    
}