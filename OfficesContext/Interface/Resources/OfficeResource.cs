namespace workstation_backend.OfficesContext.Interface.Resources;

public record class OfficeResource(int Id, string Location, int Capacity, int CostPerDay, bool Available);
