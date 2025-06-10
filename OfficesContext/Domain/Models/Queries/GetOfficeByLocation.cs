namespace workstation_backend.OfficesContext.Domain.Models.Queries;

public record GetOfficeByLocation
{
        public GetOfficeByLocation(string officeLocation)
        {
            OfficeLocation = officeLocation;
        }

        public string OfficeLocation { get; init; }
}
