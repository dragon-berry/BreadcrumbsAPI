namespace BreadcrumbsAPI.Models.Dtos;

public class CrumbDto
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Song { get; set; }
    public byte[]? Image { get; set; }
    public int Likes { get; set; } = 0;
    public DateTime? EndDate { get; set; }

    // Foreign keys
    public Guid? LifeSpanCvId { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? CrumbTypeCvId { get; set; }
    public Guid? GroupId { get; set; }

    public virtual CodeValueDto? LifeSpanCv { get; set; }
    public virtual LocationDto? Location { get; set; }
    public virtual CodeValueDto? CrumbTypeCv { get; set; }
    public virtual GroupDto? Group { get; set; }
}
