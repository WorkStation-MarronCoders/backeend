namespace workstation_backend.OfficesContext.Domain.Models.Commands;

public class CreateRatingCommand
{
    public int Score { get; set; }
    public string Comment { get; set; }
    public Guid OfficeId { get; set; }
}