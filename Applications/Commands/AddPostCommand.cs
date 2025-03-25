namespace BreadcrumbsAPI.Applications.Commands;

public class AddPostCommand : IRequest<PostDto>
{
    public required PostDto PostDto { get; set; }
}

public class AddPostCommandHandler : IRequestHandler<AddPostCommand, PostDto>
{
    private readonly IPostsRepository postsRepository;
    public AddPostCommandHandler(IPostsRepository _postsRepository)
    {
        postsRepository = _postsRepository;
    }

    public async Task<PostDto> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await postsRepository.AddPost(request.PostDto);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
