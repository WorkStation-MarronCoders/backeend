using System;
using workstation_backend.Shared.Domain.Model;

namespace workstation_backend.OfficesContext.Domain.Models.Entities;

public class OfficeService : BaseEntity
{

    public OfficeService(string name, string description, int cost, Office office)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Cost = cost;
        Office = office ?? throw new ArgumentNullException(nameof(office));
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public Office Office;
    public Guid OfficeId { get; set; }
    
}
