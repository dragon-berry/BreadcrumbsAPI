namespace BreadcrumbsAPI.Applications.Groups.Commands;

public class AddUserToGroupCommand : IRequest<bool>
{
    public required string GroupCode { get; set; }
}

public class AddUserToGroupCommandHandler : IRequestHandler<AddUserToGroupCommand, bool>
{
    private readonly IGroupRepository groupRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    public AddUserToGroupCommandHandler(IGroupRepository _groupRepository, IHttpContextAccessor _httpContextAccessor)
    {
        groupRepository = _groupRepository;
        httpContextAccessor = _httpContextAccessor;
    }

    public async Task<bool> Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(httpContextAccessor);
            var result = await groupRepository.AddUserToGroup(request.GroupCode, userId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
