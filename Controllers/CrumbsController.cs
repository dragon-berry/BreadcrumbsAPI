namespace BreadcrumbsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CrumbsController : ControllerBase
{
    private readonly IMediator mediator;

    public CrumbsController(IMediator _mediator)
    {
        mediator = _mediator;
    }

    [HttpGet("GetCrumbs")]
    public async Task<ActionResult<List<CrumbDto>>> GetCrumbs()
    {
        var result = await mediator.Send(new GetCrumbsQuery());
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpPost("AddCrumb")]
    public async Task<ActionResult<CrumbDto>> AddCrumb([FromBody] CrumbDto CrumbDto)
    {
        var result = await mediator.Send(new AddCrumbCommand { CrumbDto = CrumbDto });
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpPut("UpdateCrumb")]
    public async Task<ActionResult<CrumbDto>> UpdateCrumb([FromBody] CrumbDto CrumbDto)
    {
        var result = await mediator.Send(new UpdateCrumbCommand { CrumbDto = CrumbDto });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpDelete("DeleteCrumb/{CrumbId}")]
    public async Task<ActionResult<CrumbDto>> DeleteCrumb(Guid CrumbId)
    {
        var result = await mediator.Send(new DeleteCrumbCommand { CrumbId = CrumbId });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }
}
