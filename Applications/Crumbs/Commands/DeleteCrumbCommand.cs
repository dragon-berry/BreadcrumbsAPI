namespace BreadcrumbsAPI.Applications.Crumbs.Commands;

public class DeleteCrumbCommand : IRequest<bool>
{
    public required Guid CrumbId { get; set; }
}

public class DeleteCrumbCommandHandler : IRequestHandler<DeleteCrumbCommand, bool>
{
    private readonly ICrumbRepository crumbsRepository;
    public DeleteCrumbCommandHandler(ICrumbRepository _crumbsRepository)
    {
        crumbsRepository = _crumbsRepository;
    }

    public async Task<bool> Handle(DeleteCrumbCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await crumbsRepository.DeleteCrumb(request.CrumbId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
