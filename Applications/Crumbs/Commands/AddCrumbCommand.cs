namespace BreadcrumbsAPI.Applications.Crumbs.Commands;

public class AddCrumbCommand : IRequest<CrumbDto>
{
    public required CrumbDto CrumbDto { get; set; }
}

public class AddCrumbCommandHandler : IRequestHandler<AddCrumbCommand, CrumbDto>
{
    private readonly ICrumbRepository crumbsRepository;
    public AddCrumbCommandHandler(ICrumbRepository _crumbsRepository)
    {
        crumbsRepository = _crumbsRepository;
    }

    public async Task<CrumbDto> Handle(AddCrumbCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await crumbsRepository.AddCrumb(request.CrumbDto);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
