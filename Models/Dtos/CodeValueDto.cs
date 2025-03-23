namespace BreadcrumbsAPI.Models.Dtos;

public class CodeValueDto
{
    public Guid? Id { get; set; }
    public string GroupName { get; set; } = null!;
    public required string Value { get; set; }
    public Guid? ParentCvId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public virtual CodeValueDto? ParentCv { get; set; }
}
