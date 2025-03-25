namespace BreadcrumbsAPI.Applications.Commands;

public class UpdatePostCommand : IRequest<bool>
{
    public required PostDto PostDto { get; set; }
}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, bool>
{
    private readonly IPostsRepository postsRepository;
    public UpdatePostCommandHandler(IPostsRepository _postsRepository)
    {
        postsRepository = _postsRepository;
    }

    public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await postsRepository.UpdatePost(request.PostDto);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}