using System;
using workstation_backend.UserContext.Domain;
using workstation_backend.UserContext.Domain.Models.Entities;
using workstation_backend.UserContext.Domain.Models.Queries;
using workstation_backend.UserContext.Domain.Services;

namespace workstation_backend.UserContext.Application.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        var users = await _userRepository.GetAllAsync();
        return users;
    }

    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        var user = await _userRepository.FindByIdAsync(query.UserId);
        return user;
    }
}



    

