using System.Data;
using FluentValidation;
using workstation_backend.Shared.Domain.Repositories;
using workstation_backend.UserContext.Domain;
using workstation_backend.UserContext.Domain.Models.Commands;
using workstation_backend.UserContext.Domain.Models.Entities;
using workstation_backend.UserContext.Domain.Models.Exceptions;
using workstation_backend.UserContext.Domain.Services;

namespace workstation_backend.UserContext.Application.CommandServices;

public class UserCommandService(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork, 
    IValidator<CreateUserCommand> validator) : IUserCommandService
{
    
    private readonly IUserRepository _userRepository =
        userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IValidator<CreateUserCommand> _validator = validator;
    

    public async Task<User> Handle(CreateUserCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }
        
        var existingUser = await _userRepository.GetByDniAsync(command.Dni);
        if (existingUser is not null)
            throw new DuplicateNameException($"A user with DNI '{command.Dni}' already exists.");

        var user = new User(command.FirstName, command.LastName, command.Dni, command.PhoneNumber, command.Email,
            command.Role);
           
       await _userRepository.AddAsync(user);
       await _unitOfWork.CompleteAsync();    
       return user;
    }

    public async Task Handle(DeleteUserCommand command)
    {
        var user = await _userRepository.GetByDniAsync(command.Id.ToString())
                   ?? throw new UserNotFoundException(command.Id);

        _userRepository.Remove(user);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> Handle(UpdateSeekerCommand command, Guid userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
            if (user is null) throw new DataException("User does not exist.");
            
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.PhoneNumber = command.PhoneNumber;
            user.Email = command.Email;
            user.PhoneNumber = command.PhoneNumber;
            user.Role = command.Role;
            
            _userRepository.Update(user);
            await _unitOfWork.CompleteAsync();
            
            return true;
    }

    public async Task<bool> Handle(UpdateLessorCommand command,Guid userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user is null) throw new DataException("User does not exist.");
        
        user.FirstName = command.FirstName;
        user.
    }
}
