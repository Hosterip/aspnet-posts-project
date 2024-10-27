﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Reviews.Commands.CreateReview;
using PostsApp.Application.Reviews.Commands.DeleteReview;
using PostsApp.Application.Reviews.Commands.UpdateReview;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Review;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public class ReviewsController : ApiController
{
    private readonly ISender _sender;

    public ReviewsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(ApiRoutes.Reviews.Create)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Create(
        [FromBodyOrDefault] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateReviewCommand
        {
            BookId = request.BookId,
            UserId = new Guid(HttpContext.GetId()!),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut(ApiRoutes.Reviews.Update)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Update(
        [FromBodyOrDefault] UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand
        {
            ReviewId = request.ReviewId,
            UserId = new Guid(HttpContext.GetId()!),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete(ApiRoutes.Reviews.Delete)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand
        {
            ReviewId = id,
            UserId = new Guid(HttpContext.GetId()!)
        };
        await _sender.Send(command, cancellationToken);
        return Ok("Review was successfully deleted");
    }
}