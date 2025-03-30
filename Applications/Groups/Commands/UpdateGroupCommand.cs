namespace BreadcrumbsAPI.Applications.Groups.Commands;

public class UpdateGroupCommand : IRequest<bool>
{
    public required GroupDto GroupDto { get; set; }
}

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, bool>
{
    private readonly IGroupRepository groupRepository;
    public UpdateGroupCommandHandler(IGroupRepository _groupRepository)
    {
        groupRepository = _groupRepository;
    }

    public async Task<bool> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await groupRepository.UpdateGroup(request.GroupDto);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
