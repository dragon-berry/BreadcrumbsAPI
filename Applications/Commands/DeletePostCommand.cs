namespace BreadcrumbsAPI.Applications.Commands;

public class DeletePostCommand : IRequest<bool>
{
    public required Guid PostId { get; set; }
}

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
{
    private readonly IPostsRepository postsRepository;
    public DeletePostCommandHandler(IPostsRepository _postsRepository)
    {
        postsRepository = _postsRepository;
    }

    public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await postsRepository.DeletePost(request.PostId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
