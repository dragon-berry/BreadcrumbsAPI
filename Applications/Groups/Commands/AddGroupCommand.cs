namespace BreadcrumbsAPI.Applications.Groups.Commands;

public class AddGroupCommand : IRequest<GroupDto>
{
    public required GroupDto GroupDto { get; set; }
}

public class AddGroupCommandHandler : IRequestHandler<AddGroupCommand, GroupDto>
{
    private readonly IGroupRepository groupRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    public AddGroupCommandHandler(IGroupRepository _groupRepository, IHttpContextAccessor _httpContextAccessor)
    {
        groupRepository = _groupRepository;
        httpContextAccessor = _httpContextAccessor;
    }

    public async Task<GroupDto> Handle(AddGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = ClaimsHelper.GetUserId(httpContextAccessor);
            var result = await groupRepository.AddGroup(request.GroupDto, userId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
