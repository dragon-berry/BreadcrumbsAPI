namespace BreadcrumbsAPI.Models;

public class CodeValue : BaseEntity
{
    [Column(TypeName = "varchar(50)")]
    public string GroupName { get; set; } = null!;

    [Column(TypeName = "varchar(255)")]
    public required string Value { get; set; }
    public Guid? ParentCvId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public virtual CodeValue? ParentCv { get; set; }
}
