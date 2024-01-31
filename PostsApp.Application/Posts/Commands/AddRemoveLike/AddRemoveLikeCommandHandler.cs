using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Posts.Commands.AddRemoveLike;

public class AddRemoveLikeCommandHandler : IRequestHandler<AddRemoveLikeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddRemoveLikeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(AddRemoveLikeCommand request, CancellationToken cancellationToken)
    {
        if (
            await _unitOfWork.Likes
                .AnyAsync(like => like.User.Id == request.UserId && like.Post.Id == request.PostId))
        {
            var like = await _unitOfWork.Likes.GetSingleWhereAsync(like =>
                like.User.Id == request.UserId && like.Post.Id == request.PostId);
            await _unitOfWork.Likes.RemoveAsync(like!);
            return;
        }

        var post = await _unitOfWork.Posts.GetSingleWhereAsync(post => post.Id == request.PostId);
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
        var newLike = new Like{User = user!, Post = post!};
        await _unitOfWork.Likes.AddAsync(newLike);
    }
}