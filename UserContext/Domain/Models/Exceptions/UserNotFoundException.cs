namespace workstation_backend.UserContext.Domain.Models.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId)
        : base($"User with ID {userId} was not found.") { }

    public UserNotFoundException(string message)
        : base(message) { }

    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}