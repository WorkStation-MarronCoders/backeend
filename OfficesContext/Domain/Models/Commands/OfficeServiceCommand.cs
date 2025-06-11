using workstation_backend.OfficesContext.Domain.Models.Entities;

namespace workstation_backend.OfficesContext.Domain.Models.Commands;

public record OfficeServiceCommand(string Name, string Description, int Cost, Office office);
