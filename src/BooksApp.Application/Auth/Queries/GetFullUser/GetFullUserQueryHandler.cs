﻿using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Auth.Queries.IsSessionValid;

internal sealed class GetFullUserQueryHandler : IRequestHandler<GetFullUserQuery, User?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFullUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<User?> Handle(GetFullUserQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
    }
}