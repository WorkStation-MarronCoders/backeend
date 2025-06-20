using workstation_backend.UserContext.Domain.Models.Entities;

namespace workstation_backend.UserContext.Domain.Models.Commands;

public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Dni,
    string PhoneNumber,
    string Email,
    UserRole role
);
