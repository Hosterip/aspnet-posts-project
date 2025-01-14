﻿using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;

public sealed class PrivilegedDeleteReviewCommandValidator : AbstractValidator<PrivilegedDeleteReviewCommand>
{
    public PrivilegedDeleteReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) =>
                await unitOfWork.Reviews.AnyById(reviewId, cancellationToken))
            .WithMessage(ReviewValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(UserValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(userId, cancellationToken);

                return user?.Role.Name is RoleNames.Admin or RoleNames.Moderator;
            })
            .WithMessage(UserValidationMessages.Permission);
    }
}