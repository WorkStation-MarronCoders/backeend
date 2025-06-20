namespace workstation_backend.UserContext.Domain.Models.Commands;

public record UpdateLessorCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Dni,
    string PhoneNumber,
    string Email,
    string BusinessName);
