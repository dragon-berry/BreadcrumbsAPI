namespace BreadcrumbsAPI.Applications.Groups.Commands;

public class LeaveGroupCommand : IRequest<bool>
{
    public required Guid GroupId { get; set; }
}

public class LeaveGroupCommandHandler : IRequestHandler<LeaveGroupCommand, bool>
{
    private readonly IGroupRepository groupRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    public LeaveGroupCommandHandler(IGroupRepository _groupRepository, IHttpContextAccessor _httpContextAccessor)
    {
        groupRepository = _groupRepository;
        httpContextAccessor = _httpContextAccessor;
    }

    public async Task<bool> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(httpContextAccessor);
            var result = await groupRepository.LeaveGroup(request.GroupId, userId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}