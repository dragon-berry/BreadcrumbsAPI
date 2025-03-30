namespace BreadcrumbsAPI.Applications.Groups.Queries;

public class GetGroupsQuery : IRequest<List<GroupDto>>
{
}

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, List<GroupDto>>
{
    private readonly IGroupRepository groupRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    public GetGroupsQueryHandler(IGroupRepository _groupRepository, IHttpContextAccessor _httpContextAccessor)
    {
        groupRepository = _groupRepository;
        httpContextAccessor = _httpContextAccessor;
    }

    public async Task<List<GroupDto>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(httpContextAccessor);
            var result = await groupRepository.GetGroups(userId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
