using System;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.OfficesContext.Domain.Models.Queries;

namespace workstation_backend.OfficesContext.Domain.Services;

public interface IOfficeQueryService
{
    Task<IEnumerable<Office>> Handle(GetAllOfficesQuery query);
    Task<Office> Handle(GetOfficeByIdQuery query);
    Task<Office> Handle(GetOfficeByLocation query);
}
