using System;
using System.Data;
using FluentValidation;
using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Domain.Models.Commands;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.OfficesContext.Domain.Services;
using workstation_backend.Shared.Domain.Repositories;

namespace workstation_backend.OfficesContext.Application.CommandServices;

/// <summary>
/// Servicio encargado de manejar los comandos relacionados con la entidad <see cref="Office"/>.
/// Implementa operaciones de creación, actualización y eliminación lógica de oficinas.
/// </summary>
public class OfficeCommandService(IOfficeRepository officeRepository, IUnitOfWork unitOfWork, IValidator<CreateOfficeCommand> validator) : IOfficeCommandService
{
    private readonly IOfficeRepository _officeRepository =
           officeRepository ?? throw new ArgumentNullException(nameof(officeRepository));

    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IValidator<CreateOfficeCommand> _validator = validator
        ?? throw new ArgumentNullException(nameof(validator));

    /// <summary>
    /// Maneja la creación de una nueva oficina, incluyendo validación y verificación de duplicados.
    /// </summary>
    /// <param name="command">Comando con los datos de la oficina a crear.</param>
    /// <returns>La oficina creada.</returns>
    /// <exception cref="ArgumentNullException">Si el comando o la lista de servicios es nula.</exception>
    /// <exception cref="ValidationException">Si la validación del comando falla.</exception>
    /// <exception cref="DuplicateNameException">Si ya existe una oficina con la misma ubicación.</exception>
    public async Task<Office> Handle(CreateOfficeCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }

        var existingOffice = await _officeRepository.GetByLocationAsync(command.Location);
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
            office.Services.Add(new OfficeService(service.Name, service.Description, service.Cost));
        });

        await _officeRepository.AddAsync(office);
        await _unitOfWork.CompleteAsync();

        return office;
    }

    /// <summary>
    /// Maneja la eliminación lógica de una oficina estableciendo su estado como inactivo.
    /// </summary>
    /// <param name="command">Comando con el ID de la oficina a eliminar.</param>
    /// <returns>True si la operación fue exitosa; false si no se encontró la oficina.</returns>
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

    /// <summary>
    /// Maneja la actualización de una oficina existente.
    /// </summary>
    /// <param name="command">Comando con los nuevos datos de la oficina.</param>
    /// <param name="Id">Identificador de la oficina que se desea actualizar.</param>
    /// <returns>True si la operación fue exitosa; lanza excepción si no se encuentra la oficina.</returns>
    /// <exception cref="DataException">Si no se encuentra la oficina especificada.</exception>
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
