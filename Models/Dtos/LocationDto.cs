namespace BreadcrumbsAPI.Models.Dtos;

public class LocationDto
{
    public Guid? Id { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? StateProvince { get; set; }
    public string? Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? GoogleMaps { get; set; }

    // Foreign keys
    public Guid? LocationTypeCvId { get; set; }
    public virtual CodeValueDto? LocationTypeCv { get; set; }
}
