namespace BreadcrumbsAPI.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly BreadcrumbsDbContext context;

    public PostsRepository(BreadcrumbsDbContext _context)
    {
        context = _context;
    }

    public async Task<List<PostDto>> GetPosts()
    {
        try
        {
            var posts = await context.Posts
                .Include(p => p.Location)
                .Include(p => p.PostTypeCv)
                .Include(p => p.Group)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            return posts.Adapt<List<PostDto>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<PostDto> AddPost(PostDto postDto)
    {
        try
        {
            var post = postDto.Adapt<Post>();
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync(new CancellationToken());
            return post.Adapt<PostDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdatePost(PostDto postDto)
    {
        try
        {
            var post = await GetPost(context, postDto.Id);
            post = postDto.Adapt<Post>();

            context.Posts.Update(post);
            
            await context.SaveChangesAsync(new CancellationToken());
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> DeletePost(Guid id)
    {
        try
        {
            var post = await GetPost(context, id);
            
            post.IsDeleted = true;
            context.Posts.Update(post);

            await context.SaveChangesAsync(new CancellationToken());
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private async Task<Post> GetPost(BreadcrumbsDbContext context, Guid id)
    {
        return await context.Posts
            .Include(p => p.Location)
            .Include(p => p.PostTypeCv)
            .Include(p => p.Group)
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception();
    }
}
