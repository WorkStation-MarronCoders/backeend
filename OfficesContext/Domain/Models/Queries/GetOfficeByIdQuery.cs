namespace workstation_backend.OfficesContext.Domain.Models.Queries;

public record class GetOfficeByIdQuery
{
    public GetOfficeByIdQuery(Guid officeId)
    {
        OfficeId = officeId;
    }

    public Guid OfficeId { get; init; }
}
