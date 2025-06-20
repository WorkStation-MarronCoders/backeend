using System;
using workstation_backend.Shared.Domain.Model;

namespace workstation_backend.OfficesContext.Domain.Models.Entities;

public class OfficeService : BaseEntity
{
    public OfficeService(string name, string description, int cost)
    {
        Name = name;
        Description = description;
        IsActive = true;
        Cost = cost;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }

    public Guid OfficeId { get; set; }
    public Office Office { get; set; }
}
