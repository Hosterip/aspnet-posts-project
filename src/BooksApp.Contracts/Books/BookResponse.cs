using BooksApp.Contracts.Genres;
using BooksApp.Contracts.Users;

namespace BooksApp.Contracts.Books;

public class BookResponse
{
    public required Guid Id { get; init; }
    public required UserResponse Author { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Slug { get; init; }
    public required string Cover { get; init; }
    public required IEnumerable<GenreResponse> Genres { get; init; } = [];
}