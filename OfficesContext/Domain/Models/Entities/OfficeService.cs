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
    public string Name;
    public string Description;
    public int Cost;
    public Office Office;
    
}
