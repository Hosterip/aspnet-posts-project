using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Auth.Queries.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Username)
            .NotEmpty().Length(0, 255);
        RuleFor(user => user.Username)
            .MustAsync(async (username, cancellationToken) =>
            {
                return await unitOfWork.User.AnyAsync(user => user.Username == username);
            }).WithMessage(AuthExceptionConstants.NotFound);
        RuleFor(user => user.Password)
            .NotEmpty();
    }
}