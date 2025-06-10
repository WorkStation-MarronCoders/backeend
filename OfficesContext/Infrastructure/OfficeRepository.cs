using System;
using Microsoft.EntityFrameworkCore;
using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.Shared.Infrastructure.Persistence.Configuration;
using workstation_backend.Shared.Infrastructure.Persistence.Repositories;

namespace workstation_backend.OfficesContext.Infrastructure;

public class OfficeRepository(WorkstationContext context) : BaseRepository<Office>(context), IOfficeRepository
{
    public async Task<Office?> GetByLocationAsync(string location)
    {
        return await context.Set<Office>().FirstOrDefaultAsync(office => office.Location == location);
    }
}
