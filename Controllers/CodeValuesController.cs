namespace BreadcrumbsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CodeValuesController : Controller
{
    private readonly IMediator mediator;

    public CodeValuesController(IMediator _mediator)
    {
        mediator = _mediator;
    }

    [HttpGet("GetCodeValues")]
    public async Task<ActionResult<List<CodeValueDto>>> GetCodeValues()
    {
        var result = await mediator.Send(new GetCodeValuesQuery());
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpGet("GetCodeValuesByGroupName/{groupName}")]
    public async Task<ActionResult<List<CodeValueDto>>> GetCodeValuesByGroupName(string groupName)
    {
        var result = await mediator.Send(new GetCodeValuesByGroupNameQuery { GroupName = groupName });
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }
}
