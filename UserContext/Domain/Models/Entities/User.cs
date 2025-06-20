using System;
using workstation_backend.Shared.Domain.Model;

namespace workstation_backend.UserContext.Domain.Models.Entities;

public class User : BaseEntity
{
    public User(string firstName, string lastName, string dni, string phoneNumber, string email, UserRole role)
    {
        FirstName = firstName;
        LastName = lastName;
        Dni = dni;
        PhoneNumber = phoneNumber;
        Email = email;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Dni { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }

    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public SeekerProfile? SeekerProfile { get; private set; }
    public LessorProfile? LessorProfile { get; private set; }
}