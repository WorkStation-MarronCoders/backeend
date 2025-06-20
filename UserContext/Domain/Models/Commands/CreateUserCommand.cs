using workstation_backend.UserContext.Domain.Models.Entities;
namespace workstation_backend.UserContext.Domain.Models.Commands;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Dni,
    string PhoneNumber,
    string Email,
    UserRole Role,
    string? Nickname,       
    string? Description,    
    string? BusinessName,   
    string? ExtraInfo       
);