namespace workstation_backend.OfficesContext.Domain.Models.Queries;

public record class GetOfficeByIdQuery
{
        public GetOfficeByIdQuery(int officeId)
        {
            OfficeId = officeId;
        }

        public int OfficeId { get; init; }
}
