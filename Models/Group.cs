namespace BreadcrumbsAPI.Models;

public class Group : BaseEntity
{
    [Column(TypeName = "varchar(100)")]
    public string? Name { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? Description { get; set; }

    [Column(TypeName = "varbinary(max)")]
    public byte[]? ImageData { get; set; }
}
