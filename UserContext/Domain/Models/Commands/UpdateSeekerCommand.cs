namespace workstation_backend.UserContext.Domain.Models.Commands;

public record UpdateSeekerCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Dni,
    string PhoneNumber,
    string Email,
    string Nickname,
    string Description
);
