namespace workstation_backend.OfficesContext.Domain.Models.Commands;

public record CreateOfficeCommand(string Location, int Capacity, int CostPerDay, bool Available, List<OfficeServiceCommand> Services);
