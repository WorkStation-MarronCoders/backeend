using System;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.Shared.Domain.Repositories;

namespace workstation_backend.OfficesContext.Domain;

public interface IOfficeRepository : IBaseRepository<Office>
{
    Task<Office?> GetByLocationAsync(string location);
}
