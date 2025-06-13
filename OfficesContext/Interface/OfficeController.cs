using System;

namespace workstation_backend.OfficesContext.Interface;

using Microsoft.AspNetCore.Mvc;
using System.Data;
using workstation_backend.OfficesContext.Domain.Models.Commands;
using workstation_backend.OfficesContext.Domain.Models.Exceptions;
using workstation_backend.OfficesContext.Domain.Models.Queries;
using workstation_backend.OfficesContext.Domain.Services;
using workstation_backend.OfficesContext.Interface.Transform;

[Route("api/workstation/[controller]")]
[ApiController]
public class OfficeController(IOfficeQueryService officeQueryService, IOfficeCommandService officeCommandService) : ControllerBase
{
    private readonly IOfficeQueryService _officeQueryService = officeQueryService ?? throw new ArgumentNullException(nameof(officeQueryService));
    private readonly IOfficeCommandService _officeCommandService = officeCommandService ?? throw new ArgumentNullException(nameof(officeCommandService));

    public async Task<IActionResult> GetAsync()
    {
        var query = new GetAllOfficesQuery();
        var result = await _officeQueryService.Handle(query);

        if (!result.Any()) return NotFound("No offices found.");

        var resources = result.Select(OfficeResourceFromEntityAssembler.ToResourceFromEntity).ToList();
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(Guid id)
    {
       if (id == Guid.Empty)
        return BadRequest("Invalid book ID.");

        try
        {
            var query = new GetOfficeByIdQuery(id);
            var result = await _officeQueryService.Handle(query);

            return result != null ? Ok(OfficeResourceFromEntityAssembler.ToResourceFromEntity(result)) : NotFound($"Office with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{location:string}")]
    public async Task<IActionResult> Get(string location)
    {
        if (string.IsNullOrWhiteSpace(location)) return BadRequest("Invalid location");

        try
        {
            var query = new GetOfficeByLocation(location);
            var result = await _officeQueryService.Handle(query);

            return result != null ? Ok(OfficeResourceFromEntityAssembler.ToResourceFromEntity(result)) : NotFound($"Office with this lcocation {location} not found.");
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    // POST: api/Book
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateOfficeCommand command)
    {
        if (command == null) return BadRequest("Invalid office data.");

        try
        {
            await _officeCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotServicesFoundException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (DuplicateNameException)
        {
            return Conflict("An office with the same location already exists.");
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateOfficeCommand UpdateOfficeCommand)
    {
        try
        {
            await _officeCommandService.Handle(UpdateOfficeCommand, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid book ID.");

            try
            {
                var command = new DeleteOfficeCommand(id);
                await _officeCommandService.Handle(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


}
