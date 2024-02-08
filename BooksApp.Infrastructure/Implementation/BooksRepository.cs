using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation;

public class BooksRepository : GenericRepository<Book>, IPostsRepository
{
    public BooksRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page, string query) 
    {
        return await 
            (
                from book in _dbContext.Books
                where query == null || book.Title.Contains(query)
                let user = new UserResult { Id = book.Author.Id, Username = book.Author.Username }
                join like in _dbContext.Likes.Include(like => like.Book)
                    on book.Id equals like.Book.Id into likes
                select new BookResult { Id = book.Id, Title = book.Title, Description = book.Description, Author = user, LikeCount = likes.Count()})
            .PaginationAsync(page, limit);
    }

    public async Task<IEnumerable<BookResult>> GetBooks(Expression<Func<Book, bool>> expression)
    {
        return (
            from post in _dbContext.Books
                .Where(expression)
                .Include(book => book.Author)
            
            join like in _dbContext.Likes.Include(like => like.Book)
                on post.Id equals like.Book.Id into likes 
            let user = new UserResult{Id = post.Author.Id, Username = post.Author.Username }
            select new BookResult { Id = post.Id, Title = post.Title, Description = post.Description, LikeCount = likes.Count(), Author = user }
        );
    }

    public override async Task<Book?> GetSingleWhereAsync(Expression<Func<Book, bool>> expression)
    {
        var post = await _dbContext.Books
            .Include(post => post.Author)
            .SingleAsync(expression);

        return post;
    }

    public override async Task<bool> AnyAsync(Expression<Func<Book, bool>> expression)
    {
        return await _dbContext.Books
            .Include(post => post.Author)
            .AnyAsync(expression);
    }
}