using System;
using workstation_backend.OfficesContext.Interface.Resources;
using workstation_backend.OfficesContext.Domain.Models.Entities;

namespace workstation_backend.OfficesContext.Interface.Transform;

public static class OfficeResourceFromEntityAssembler
{
    public static OfficeResource ToResourceFromEntity(Office office)
    {
        return new OfficeResource(office.Id, office.Location, office.Capacity, office.CostPerDay, office.Available);
    }
}
