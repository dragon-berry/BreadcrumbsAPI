namespace BreadcrumbsAPI.Models;

public abstract class BaseEntity : IEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid CreationUserId { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }

    [Required]
    public Guid UpdatedUserId { get; set; }

    [Required]
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}
