namespace BreadcrumbsAPI.Applications.Queries;

public class GetPostsQuery : IRequest<List<PostDto>>
{
}

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, List<PostDto>>
{
    private readonly IPostsRepository postsRepository;
    public GetPostsQueryHandler(IPostsRepository _postsRepository)
    {
        postsRepository = _postsRepository;
    }

    public async Task<List<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await postsRepository.GetPosts();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
