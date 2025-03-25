namespace BreadcrumbsAPI.Controllers;

[ApiController]
[Route("Posts")]
public class PostController : ControllerBase
{
    private readonly IMediator mediator;

    public PostController(IMediator _mediator)
    {
        mediator = _mediator;
    }

    [HttpGet("GetPosts")]
    public async Task<ActionResult<List<PostDto>>> GetPosts()
    {
        var result = await mediator.Send(new GetPostsQuery());
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpPost("AddPost")]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] PostDto postDto)
    {
        var result = await mediator.Send(new AddPostCommand { PostDto = postDto });
        if (result != null)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpPut("UpdatePost")]
    public async Task<ActionResult<PostDto>> UpdatePost([FromBody] PostDto postDto)
    {
        var result = await mediator.Send(new UpdatePostCommand { PostDto = postDto });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }

    [HttpDelete("DeletePost/{postId}")]
    public async Task<ActionResult<PostDto>> DeletePost(Guid postId)
    {
        var result = await mediator.Send(new DeletePostCommand { PostId = postId });
        if (result)
            return Ok(result);
        else
            return BadRequest();
    }
}
