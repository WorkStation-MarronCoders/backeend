using System;
using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.OfficesContext.Domain.Models.Queries;
using workstation_backend.OfficesContext.Domain.Services;

namespace workstation_backend.OfficesContext.Application.QueryServices;

public class OfficeQueryService : IOfficeQueryService
{
    private readonly IOfficeRepository _officeRepository;

    public OfficeQueryService(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository ?? throw new ArgumentNullException(nameof(officeRepository));
    }

    public async Task<IEnumerable<Office>> Handle(GetAllOfficesQuery query)
    {
        var offices = await _officeRepository.ListAsync();
        return offices?.Where(office => office.IsActive) ?? Enumerable.Empty<Office>();
    }

    public async Task<Office?> Handle(GetOfficeByIdQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        var office = await _officeRepository.FindByIdAsync(query.OfficeId);
        return office?.IsActive == true ? office : null;
    }
    public async Task<Office?> Handle(GetOfficeByLocation query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        var office = await _officeRepository.GetByLocationAsync(query.OfficeLocation);
        return office?.IsActive == true ? office : null;
    }
}

