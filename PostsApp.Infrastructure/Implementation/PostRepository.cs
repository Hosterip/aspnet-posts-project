using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.DB;

namespace PostsApp.Infrastructure.Implementation;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<PostResult>> GetPaginated(int limit, int page, string query) 
    {
        return await 
            (
                from post in _dbContext.Posts
                where query == null || post.Title.Contains(query)
                let user = new UserResult { Id = post.User.Id, Username = post.User.Username }
                select new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, User = user })
            .PaginationAsync(page, limit);
    }

    public async Task<PostResult> GetById(int id)
    {
        var post = await _dbContext.Posts
            .Include(post => post.User)
            .SingleAsync(post => post.Id == id);

        var user = new UserResult { Username = post.User.Username };

        return new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, User = user };
    }

    public async Task<bool> PostAny(Expression<Func<Post, bool>> expression)
    {
        return await _dbContext.Posts
            .Include(post => post.User)
            .AnyAsync(expression);
    }
}