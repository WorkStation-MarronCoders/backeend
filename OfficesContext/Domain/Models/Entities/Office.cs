using System;
using workstation_backend.Shared.Domain.Model;

namespace workstation_backend.OfficesContext.Domain.Models.Entities;

public class Office : BaseEntity
{

    public Office(string location, int capacity, int costperday, bool available)
    {
        Location = location;
        Capacity = capacity;
        CostPerDay = costperday;
        Available = available;
        Services = new List<OfficeService>();
    }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public int CostPerDay { get; set; }
    public bool Available { get; set; }
    public List<OfficeService> Services { get; } = new();
    
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

}
