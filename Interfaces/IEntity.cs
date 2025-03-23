namespace BreadcrumbsAPI.Interfaces;

public interface IEntity
{
    public Guid Id { get; set; }
    public Guid CreationUserId { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid UpdatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}
