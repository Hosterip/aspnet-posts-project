﻿using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Enums.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.UpdateReview;

public sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Body)
            .MaximumLength((int)ReviewMaxLengths.Body)
            .NotEmpty();
        RuleFor(request => request.Rating)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) => await unitOfWork.Reviews.AnyById(reviewId))
            .WithMessage(ReviewValidationMessages.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var review = await unitOfWork.Reviews.GetSingleById(request.ReviewId);
                if (review is not null)
                    return review.User.Id == UserId.CreateUserId(request.UserId);
                return true;
            }).WithMessage(ReviewValidationMessages.NotYours);
    }
}