namespace BreadcrumbsAPI.Applications.Groups.Queries;

public class GetAllGroupsQuery : IRequest<List<GroupDto>>
{
}

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, List<GroupDto>>
{
    private readonly IGroupRepository groupRepository;
    public GetAllGroupsQueryHandler(IGroupRepository _groupRepository)
    {
        groupRepository = _groupRepository;
    }

    public async Task<List<GroupDto>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await groupRepository.GetAllGroups();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}