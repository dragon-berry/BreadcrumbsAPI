namespace BreadcrumbsAPI.Models;

public class Crumb : BaseEntity
{
    [Column(TypeName = "varchar(100)")]
    public string? Title { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? Body { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? Song { get; set; }

    [Column(TypeName = "varbinary(max)")]
    public byte[]? Image { get; set; }
    public int Likes { get; set; } = 0;
    public DateTime EndDate { get; set; }

	// Foreign keys
	public Guid LifeSpanCvId { get; set; }
	public Guid LocationId { get; set; }
    public Guid CrumbTypeCvId { get; set; }
    public Guid GroupId { get; set; }

	public virtual CodeValue? LifeSpanCv { get; set; }
	public virtual Location? Location { get; set; }
    public virtual CodeValue? CrumbTypeCv { get; set; }
    public virtual Group? Group { get; set; }
}
