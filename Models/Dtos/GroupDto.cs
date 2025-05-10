namespace BreadcrumbsAPI.Models.Dtos;

public class GroupDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte[]? ImageData { get; set; }
    public string? Code { get; set; }
    public int? CrumbLimitPerDay { get; set; } = 1;

    // Foreign keys
    public Guid? LifeSpanCvId { get; set; } = CodeValueConstants.OneDayLifeSpan; //Setting default value to One Day Life Span (for testing)
    public virtual CodeValueDto? LifeSpanCv { get; set; }

    public virtual ICollection<GroupUserRelationshipDto>? GroupUserRelationships { get; set; }
}
