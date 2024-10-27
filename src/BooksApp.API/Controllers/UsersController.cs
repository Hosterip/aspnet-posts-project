﻿using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Bookshelves.Queries.GetBookshelves;
using PostsApp.Application.Roles.Commands.UpdateRole;
using PostsApp.Application.Roles.Queries.GetRoles;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Commands.InsertAvatar;
using PostsApp.Application.Users.Commands.UpdateEmail;
using PostsApp.Application.Users.Commands.UpdateName;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Role;
using PostsApp.Common.Contracts.Requests.User;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiRoutes.Users.GetMe)]
    public async Task<IActionResult> GetMe(
        CancellationToken cancellationToken)
    {
        var id = new Guid(HttpContext.GetId()!);
        var query = new GetSingleUserQuery { Id = id };
        var user = await _sender.Send(query, cancellationToken);
        return Ok(user);
    }

    [HttpGet(ApiRoutes.Users.GetMany)]
    public async Task<IActionResult> GetMany(
        int? page,
        int? limit,
        string? q,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery { Query = q, Page = page, Limit = limit };
        var users = await _sender.Send(query, cancellationToken);
        return Ok(users);
    }

    [HttpGet(ApiRoutes.Users.GetById)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleUserQuery { Id = id };
        var user = await _sender.Send(query, cancellationToken);
        return Ok(user);
    }

    [HttpDelete(ApiRoutes.Users.Delete)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> Delete(
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { Id = new Guid(HttpContext.GetId()!) };
        await _sender.Send(command, cancellationToken);

        await HttpContext.SignOutAsync();

        return Ok("User was deleted");
    }

    [HttpPut(ApiRoutes.Users.UpdateEmail)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateEmail(
        [FromBodyOrDefault] UpdateEmail request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmailCommand
        {
            Id = new Guid(HttpContext.GetId()!),
            Email = request.Email,
        };

        await _sender.Send(command, cancellationToken);

        HttpContext.ChangeEmail(request.Email);

        return Ok("Email was updated");
    }

    [HttpPut(ApiRoutes.Users.UpdateName)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateName(
        [FromBodyOrDefault] UpdateName request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNameCommand
        {
            UserId = new Guid(HttpContext.GetId()!),
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
        };

        await _sender.Send(command, cancellationToken);

        return Ok("Name was updated");
    }
    
    [HttpPut(ApiRoutes.Users.UpdateAvatar)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateAvatar(
        [FromBodyOrDefault] InsertAvatarRequest request,
        CancellationToken cancellationToken)
    {
        
        var command = new InsertAvatarCommand
        {
            Id = new Guid(HttpContext.GetId()!),
            Image = request.Image
        };

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    // Roles Logic
    [HttpPut(ApiRoutes.Users.UpdateRole)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateRole(
        [FromBodyOrDefault] ChangeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = new Guid(HttpContext.GetId()!), 
            Role = request.Role, 
            UserId = request.UserId
        };

        await _sender.Send(command, cancellationToken);
        
        return Ok("Operation succeeded");
    }
    

    [HttpGet(ApiRoutes.Users.GetRoles)]
    public async Task<IActionResult> GetRoles(
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await _sender.Send(command, cancellationToken);

        return Ok(roles);
    }
    
    // Bookshelves 
    
    [HttpGet(ApiRoutes.Users.GetBookshelves)]
    public async Task<IActionResult> GetBookshelves(
        Guid userId)
    {
        var query = new GetBookshelvesQuery
        {
            UserId = userId
        };
        var result = await _sender.Send(query);

        return Ok(result);
    }
    
    // Books 
    
    [HttpGet(ApiRoutes.Users.GetManyBooks)]
    public async Task<IActionResult> GetManyBooks(
        CancellationToken cancellationToken,
        [FromRoute] Guid userId,
        [FromQuery] int? page,
        [FromQuery] int? limit,
        [FromQuery] string? q,
        [FromQuery] Guid? genreId
    )
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page, UserId = userId, GenreId = genreId};
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}