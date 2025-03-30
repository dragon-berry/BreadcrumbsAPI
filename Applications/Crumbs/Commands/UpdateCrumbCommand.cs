namespace BreadcrumbsAPI.Applications.Crumbs.Commands;

public class UpdateCrumbCommand : IRequest<bool>
{
    public required CrumbDto CrumbDto { get; set; }
}

public class UpdateCrumbCommandHandler : IRequestHandler<UpdateCrumbCommand, bool>
{
    private readonly ICrumbRepository crumbsRepository;
    public UpdateCrumbCommandHandler(ICrumbRepository _crumbsRepository)
    {
        crumbsRepository = _crumbsRepository;
    }

    public async Task<bool> Handle(UpdateCrumbCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await crumbsRepository.UpdateCrumb(request.CrumbDto);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}