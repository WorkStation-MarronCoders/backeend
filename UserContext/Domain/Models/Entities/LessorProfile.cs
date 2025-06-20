using System;
using workstation_backend.Shared.Domain.Model;

namespace workstation_backend.UserContext.Domain.Models.Entities;

public class LessorProfile : BaseEntity
{
    public LessorProfile(Guid userId, string businessName)
    {
        UserId = userId;
        BusinessName = businessName;
    }

    public Guid UserId { get; private set; }
    public string BusinessName { get; private set; }

    public User? User { get; private set; }
}