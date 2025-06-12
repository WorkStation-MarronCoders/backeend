using System;
using System.Data;
using FluentValidation;
using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Domain.Models.Commands;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.OfficesContext.Domain.Services;
using workstation_backend.Shared.Domain.Repositories;

namespace workstation_backend.OfficesContext.Application.CommandServices;

public class OfficeCommandService(IOfficeRepository officeRepository, IUnitOfWork unitOfWork, IValidator<CreateOfficeCommand> validator) : IOfficeCommandService
{
    private readonly IOfficeRepository _officeRepository =
           officeRepository ?? throw new ArgumentNullException(nameof(officeRepository));

    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IValidator<CreateOfficeCommand> _validator = validator

        ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Office> Handle(CreateOfficeCommand command)
    {

        ArgumentNullException.ThrowIfNull(command);


        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }

        var existingOffice =
            await _officeRepository.GetByLocationAsync(command.Location);
        if (existingOffice != null)
            throw new DuplicateNameException($"An office with this location '{command.Location}' already exists.");

        if (command.Services == null || !command.Services.Any())
            throw new ArgumentNullException();

        var office = new Office(command.Location, command.Capacity, command.CostPerDay, command.Available)
        {
            UserId = 1
        };

        command.Services.ForEach(service =>
        {
            office.Services.Add(new OfficeService(service.Name, service.Description, service.Cost, service.office));
        });

        await _officeRepository.AddAsync(office);
        await _unitOfWork.CompleteAsync();

        return office;
    }
    public async Task<bool> Handle(DeleteOfficeCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var office = await _officeRepository.FindByIdAsync(command.Id);
            if (office is null) return false;

            office.IsActive = false;
            office.ModifiedDate = DateTime.UtcNow;
            office.UpdatedUserId = 87;

            _officeRepository.Update(office);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    public async Task<bool> Handle(UpdateOfficeCommand command, Guid Id)
    {
        var office = await _officeRepository.FindByIdAsync(Id);
            if (office is null) throw new DataException("Office not found.");


            office.Location = command.Location;
            office.Capacity = command.Capacity;
            office.CostPerDay = command.CostPerDay;
            office.Available = command.Available;
            office.ModifiedDate = DateTime.UtcNow;
            office.UpdatedUserId = 87;

            _officeRepository.Update(office);
            await _unitOfWork.CompleteAsync();

            return true;  
        
    }
    
}
