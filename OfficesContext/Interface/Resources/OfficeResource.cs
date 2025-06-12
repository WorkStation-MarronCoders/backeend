namespace workstation_backend.OfficesContext.Interface.Resources;

public record class OfficeResource(Guid Id, string Location, int Capacity, int CostPerDay, bool Available);
