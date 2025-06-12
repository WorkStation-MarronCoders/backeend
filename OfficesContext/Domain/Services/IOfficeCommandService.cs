using System;
using workstation_backend.OfficesContext.Domain.Models.Commands;
using workstation_backend.OfficesContext.Domain.Models.Entities;

namespace workstation_backend.OfficesContext.Domain.Services;

public interface IOfficeCommandService
{
    Task<Office> Handle(CreateOfficeCommand command);
    Task<bool> Handle(DeleteOfficeCommand command);
    Task<bool> Handle(UpdateOfficeCommand command, Guid Id);
}
