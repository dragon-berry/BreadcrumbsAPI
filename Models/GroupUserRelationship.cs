namespace BreadcrumbsAPI.Models;

public class GroupUserRelationship : BaseEntity
{
    public Guid? GroupId { get; set; }
    public Guid? UserId { get; set; }
    public bool? IsOwner { get; set; }
    public virtual Group? Group { get; set; }
    public virtual IdentityUser? User { get; set; }
}
