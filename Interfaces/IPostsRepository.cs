namespace BreadcrumbsAPI.Interfaces;

public interface IPostsRepository
{
    Task<List<PostDto>> GetPosts();
    Task<PostDto> AddPost(PostDto postDto);
    Task<bool> UpdatePost(PostDto postDto);
    Task<bool> DeletePost(Guid postId);
}
