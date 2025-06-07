using System;

namespace workstation_backend.Shared.Domain.Model;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public int UserId { get; set; }
    public int? UpdatedUserId { get; set; }
    public bool IsActive { get; set; }
}
