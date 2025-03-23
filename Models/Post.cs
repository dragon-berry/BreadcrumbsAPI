namespace BreadcrumbsAPI.Models;

public class Post : BaseEntity
{
    [Column(TypeName = "varchar(100)")]
    public string? Title { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? Body { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? Song { get; set; }

    [Column(TypeName = "varbinary(max)")]
    public byte[]? Image { get; set; }

    public int? Likes { get; set; }
    public DateTime? LifeSpan { get; set; }

    // Foreign keys
    public Guid? LocationId { get; set; }
    public Guid? PostTypeCvId { get; set; }
    public Guid? GroupId { get; set; }


    public virtual Location? Location { get; set; }
    public virtual CodeValue? PostTypeCv { get; set; }
    public virtual Group? Group { get; set; }
}
