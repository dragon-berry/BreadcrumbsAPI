namespace BreadcrumbsAPI.Models;

public class Group : BaseEntity
{
    [Column(TypeName = "varchar(100)")]
    public required string Name { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? Description { get; set; }

    [Column(TypeName = "varbinary(max)")]
    public byte[]? ImageData { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string Code { get; set; }
    public int CrumbLimitPerDay { get; set; } = 1;

    // Foreign keys
    public Guid LifeSpanCvId { get; set; }
    public virtual CodeValue? LifeSpanCv { get; set; }

    public virtual ICollection<GroupUserRelationship>? GroupUserRelationships { get; set; }
}
