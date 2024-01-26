using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Auth;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Auth.Commands.Register;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.User.AnyAsync(user => user.Username == request.Username))
            throw new AuthException("Username is occupied");
        var hashSalt = AuthUtils.CreateHashSalt(request.Password);
        User user = new User { Username = request.Username, Hash = hashSalt.hash, Salt = hashSalt.salt };
        await _unitOfWork.User.AddAsync(user);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new AuthResult {Id = user.Id, username = request.Username };
    }
}