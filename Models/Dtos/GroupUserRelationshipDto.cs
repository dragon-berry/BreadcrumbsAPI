namespace BreadcrumbsAPI.Models.Dtos;

public class GroupUserRelationshipDto
{
    public Guid? Id { get; set; }
    public Guid? GroupId { get; set; }
    public Guid? UserId { get; set; }
    public bool IsOwner { get; set; }
    public Guid? StatusCvId { get; set; } //Active, Pending, and Removed
    public virtual GroupDto? Group { get; set; }
    public virtual UserDto? User { get; set; }
    public virtual CodeValueDto? StatusCv { get; set; }
}
