namespace BreadcrumbsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator _mediator)
    {
        mediator = _mediator;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await mediator.Send(new LoginQuery { LoginDto = loginDto });
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
    {
        var result = await mediator.Send(new RegisterCommand { RegisterDto = registerDto });
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }
}
