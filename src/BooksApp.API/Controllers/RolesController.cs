using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Roles;
using BooksApp.Application.Roles.Commands.UpdateRole;
using BooksApp.Application.Roles.Queries.GetRoles;
using BooksApp.Contracts.Roles;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class RolesController(
    ISender sender,
    IMapper mapster) 
    : ApiController
{
    [HttpGet(ApiRoutes.Users.GetRoles)]
    [OutputCache]
    [ProducesResponseType(typeof(IEnumerable<RoleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResult>>> GetRoles(
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await sender.Send(command, cancellationToken);

        var response = mapster.Map<IEnumerable<RoleResponse>>(roles);
        
        return Ok(response);
    }
}