namespace BreadcrumbsAPI.Models;

public class GroupUserRelationship : BaseEntity
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
    public bool IsOwner { get; set; }
    public Guid StatusCvId { get; set; } //Active, Pending, and Removed
    public virtual Group? Group { get; set; }
    public virtual User? User { get; set; }
    public virtual CodeValue? StatusCv { get; set; }
}