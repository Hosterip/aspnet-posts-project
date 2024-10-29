using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.User.ValueObjects;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBookByName;

internal sealed class RemoveBookByNameCommandHandler : IRequestHandler<RemoveBookByNameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBookByNameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveBookByNameCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleWhereAsync(bookshelf =>
            bookshelf.Name == request.BookshelfName &&
            bookshelf.User != null &&
            bookshelf.User.Id == UserId.CreateUserId(request.UserId));
        if (bookshelf is null)
        {
            var user = await _unitOfWork.Users.GetSingleById(request.UserId);
            bookshelf = Bookshelf.Create(user!, request.BookshelfName);
            await _unitOfWork.Bookshelves.AddAsync(bookshelf);
        }

        bookshelf!.RemoveBook(request.BookId);
    }
}