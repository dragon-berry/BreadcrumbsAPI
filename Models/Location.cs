namespace BreadcrumbsAPI.Models;

public class Location : BaseEntity
{
    [Column(TypeName = "varchar(200)")]
    public string? AddressLine1 { get; set; }

    [Column(TypeName = "varchar(200)")]
    public string? AddressLine2 { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? City { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? StateProvince { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string? PostalCode { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? Country { get; set; }

    [Column(TypeName = "decimal(10,6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(10,6)")]
    public decimal? Longitude { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? GoogleMaps { get; set; }
}