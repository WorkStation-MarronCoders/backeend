using System;
using workstation_backend.UserContext.Domain.Models.Commands;
using workstation_backend.UserContext.Domain.Models.Entities;

namespace workstation_backend.UserContext.Domain.Services;

public interface IUserCommandService
{
    Task<User> Handle(CreateUserCommand command);
    Task<bool> Handle(UpdateUserCommand command, Guid id);
    Task Handle(DeleteUserCommand command);
}


