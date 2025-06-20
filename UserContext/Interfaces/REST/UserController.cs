using System;

using Microsoft.AspNetCore.Mvc;
using System.Data;
using workstation_backend.UserContext.Domain.Models.Commands;
using workstation_backend.UserContext.Domain.Models.Entities;
using workstation_backend.UserContext.Domain.Models.Exceptions;
using workstation_backend.UserContext.Domain.Models.Queries;
using workstation_backend.UserContext.Domain.Services;
using workstation_backend.UserContext.Interfaces.REST.Transform;

namespace workstation_backend.UserContext.Interfaces.REST;

[Route("api/workstation/[controller]")]
[ApiController]
public class UserController(IUserQueryService userQueryService, IUserCommandService userCommandService): ControllerBase
{
    private readonly IUserQueryService _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
    private readonly IUserCommandService _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));

    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await _userQueryService.Handle(query);
        
        if(!result.Any()) return NotFound("No users found.");
        
        var resourcers = result.Select(UserResourceFromEntityAssembler.ToResourceFromEntity).ToList();
        return Ok(resourcers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid book ID.");
        try
        {
            var query = new GetUserByIdQuery(id);
            var result = await _userQueryService.Handle(query);

            return result != null
                ? Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(result))
                : NotFound($"Office with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    // POST:
    public async Task<IActionResult> Post(CreateUserCommand command)
    {
        if (command == null) return BadRequest("Invalid user data.");

        try
        {
            await _userCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (UserNotFoundException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (DuplicateNameException)
        {
            return Conflict("An user with the same dni already exists.");
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateUserCommand UpdateUserCommand)
    {
        try
        {
            await _userCommandService.Handle(UpdateUserCommand, id);
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
            return BadRequest("Invalid user ID.");

        try
        {
            var command = new DeleteUserCommand(id);
            await _userCommandService.Handle(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
}