namespace BreadcrumbsAPI.Applications.Groups.Commands;

public class DeleteGroupCommand : IRequest<bool>
{
    public required Guid GroupId { get; set; }
}

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, bool>
{
    private readonly IGroupRepository groupRepository;
    public DeleteGroupCommandHandler(IGroupRepository _groupRepository)
    {
        groupRepository = _groupRepository;
    }

    public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await groupRepository.DeleteGroup(request.GroupId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}