namespace BreadcrumbsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IMediator mediator;

    public GroupsController(IMediator _mediator)
    {
        mediator = _mediator;
    }

    [Authorize]
    [HttpGet("GetGroups")]
    public async Task<ActionResult<List<GroupDto>>> GetGroups()
    {
        var result = await mediator.Send(new GetGroupsQuery());
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpPost("GetAllGroups")]
    public async Task<ActionResult<List<GroupDto>>> GetAllGroups()
    {
        var result = await mediator.Send(new GetAllGroupsQuery());
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [Authorize]
    [HttpPost("AddGroup")]
    public async Task<ActionResult<GroupDto>> AddGroup([FromBody] GroupDto groupDto)
    {
        var result = await mediator.Send(new AddGroupCommand { GroupDto = groupDto });
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [Authorize]
    [HttpPost("AddUserToGroup/{groupCode}")]
    public async Task<ActionResult<bool>> AddUserToGroup(string groupCode)
    {
        var result = await mediator.Send(new AddUserToGroupCommand { GroupCode = groupCode });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }

    [Authorize]
    [HttpPut("UpdateGroup")]
    public async Task<ActionResult<bool>> UpdateGroup([FromBody] GroupDto groupDto)
    {
        var result = await mediator.Send(new UpdateGroupCommand { GroupDto = groupDto });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }

    [Authorize]
    [HttpPut("LeaveGroup/{GroupId}")]
    public async Task<ActionResult<bool>> LeaveGroup(Guid groupId)
    {
        var result = await mediator.Send(new LeaveGroupCommand { GroupId = groupId });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }

    [Authorize]
    [HttpDelete("DeleteGroup/{GroupId}")]
    public async Task<ActionResult<bool>> DeleteGroup(Guid groupId)
    {
        var result = await mediator.Send(new DeleteGroupCommand { GroupId = groupId });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }
}
