namespace BreadcrumbsAPI.Models.Dtos;

public class PostDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Song { get; set; }
    public byte[]? Image { get; set; }
    public int? Likes { get; set; }
    public DateTime? LifeSpan { get; set; }

    // Foreign keys
    public Guid? LocationId { get; set; }
    public Guid? PostTypeCvId { get; set; }
    public Guid? GroupId { get; set; }
}
