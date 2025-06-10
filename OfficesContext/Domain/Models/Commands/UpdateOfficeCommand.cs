namespace workstation_backend.OfficesContext.Domain.Models.Commands;

public record UpdateOfficeCommand (int Id, string Location, int Capacity, int CostPerDay, bool Available);

