using System.Data;
using FluentValidation;
using workstation_backend.Shared.Domain.Repositories;
using workstation_backend.UserContext.Domain;
using workstation_backend.UserContext.Domain.Models.Commands;
using workstation_backend.UserContext.Domain.Models.Entities;
using workstation_backend.UserContext.Domain.Models.Exceptions;
using workstation_backend.UserContext.Domain.Services;

namespace workstation_backend.UserContext.Application.CommandServices;

/// <summary>
/// Servicio encargado de manejar comandos relacionados con usuarios,
/// como creación, actualización y eliminación.
/// </summary>
public class UserCommandService(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork, 
    IValidator<CreateUserCommand> validator) : IUserCommandService
{
    // Repositorio de usuarios para acceder a la persistencia.
    private readonly IUserRepository _userRepository =
        userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    
    // Unidad de trabajo para confirmar transacciones.
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    // Validador para la creación de usuarios.
    private readonly IValidator<CreateUserCommand> _validator = validator;

    /// <summary>
    /// Maneja la creación de un nuevo usuario, incluyendo validación y verificación de duplicados.
    /// </summary>
    /// <param name="command">Comando con los datos del usuario a crear.</param>
    /// <returns>El usuario creado.</returns>
    /// <exception cref="ValidationException">Si los datos no cumplen las reglas.</exception>
    /// <exception cref="DuplicateNameException">Si ya existe un usuario con el mismo DNI.</exception>
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

        var user = new User(
            command.FirstName, 
            command.LastName, 
            command.Dni, 
            command.PhoneNumber, 
            command.Email,
            command.Role);
           
        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();    

        return user;
    }

    /// <summary>
    /// Maneja la eliminación de un usuario del sistema.
    /// </summary>
    /// <param name="command">Comando que contiene el ID del usuario a eliminar.</param>
    /// <exception cref="UserNotFoundException">Si no se encuentra el usuario por el ID.</exception>
    public async Task Handle(DeleteUserCommand command)
    {
        var user = await _userRepository.GetByDniAsync(command.Id.ToString())
                   ?? throw new UserNotFoundException(command.Id);

        _userRepository.Remove(user);
        await _unitOfWork.CompleteAsync();
    }

    /// <summary>
    /// Maneja la actualización de datos de un usuario existente.
    /// </summary>
    /// <param name="command">Comando con los datos actualizados.</param>
    /// <param name="userId">ID del usuario a modificar.</param>
    /// <returns>True si se actualizó correctamente.</returns>
    /// <exception cref="DataException">Si no se encuentra el usuario.</exception>
    public async Task<bool> Handle(UpdateUserCommand command, Guid userId)
    {
        var user = await _userRepository.FindByIdAsync(userId)
                   ?? throw new DataException("User does not exist.");
        
        user.Update(command.FirstName, command.LastName, command.PhoneNumber, command.Email);

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}
