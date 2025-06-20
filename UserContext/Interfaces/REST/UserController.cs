using System;

using Microsoft.AspNetCore.Mvc;
using System.Data;
using workstation_backend.OfficesContext.Domain.Services;
using workstation_backend.UserContext.Domain.Models.Commands;
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
    
   
    
}