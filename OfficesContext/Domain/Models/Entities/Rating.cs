using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.Shared.Domain.Model;

public class Rating : BaseEntity
{
    public int Score { get; set; } // 
    public string Comment { get; set; }
    public Guid OfficeId { get; set; }

    public Office Office { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}