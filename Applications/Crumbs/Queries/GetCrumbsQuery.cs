namespace BreadcrumbsAPI.Applications.Crumbs.Queries;

public class GetCrumbsQuery : IRequest<List<CrumbDto>>
{
}

public class GetCrumbsQueryHandler : IRequestHandler<GetCrumbsQuery, List<CrumbDto>>
{
    private readonly ICrumbRepository crumbsRepository;
    public GetCrumbsQueryHandler(ICrumbRepository _crumbsRepository)
    {
        crumbsRepository = _crumbsRepository;
    }

    public async Task<List<CrumbDto>> Handle(GetCrumbsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await crumbsRepository.GetCrumbs();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
