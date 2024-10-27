using FluentValidation;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Enums.MaxLengths;

namespace PostsApp.Application.Users.Commands.UpdateName;

public class UpdateNameCommandValidator : AbstractValidator<UpdateNameCommand>
{
    public UpdateNameCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId));

        RuleFor(user => user.FirstName)
            .NotEmpty()
            .Length(1, (int)UserMaxLengths.FirstName);

        RuleFor(user => user.MiddleName)
            .MaximumLength((int)UserMaxLengths.MiddleName);

        RuleFor(user => user.LastName)
            .MaximumLength((int)UserMaxLengths.LastName);
    }
}