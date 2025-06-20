using System;
using workstation_backend.Shared.Domain.Model;

namespace workstation_backend.UserContext.Domain.Models.Entities;

public class SeekerProfile : BaseEntity
{
    public SeekerProfile(Guid userId, string nickname, string description)
    {
        UserId = userId;
        Nickname = nickname;
        Description = description;
    }

    public Guid UserId { get; private set; }

    public string Nickname { get; private set; }
    public string Description { get; private set; }

    public User? User { get; private set; }
}