using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetUserRelationships;

internal sealed class
    GetUserRelationshipsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserRelationshipsQuery, PaginatedArray<UserResult>>
{
    public async Task<PaginatedArray<UserResult>> Handle(
        GetUserRelationshipsQuery request,
        CancellationToken cancellationToken)
    {
        var query = request.Query ?? "";
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;

        if (request.RelationshipType == RelationshipType.Followers)
            return await unitOfWork.Users
                .GetPaginatedFollowers(request.CurrentUserId, request.UserId, page, limit, query);
        
        return await unitOfWork.Users
            .GetPaginatedFollowing(request.CurrentUserId, request.UserId, page, limit, query);
    }
}