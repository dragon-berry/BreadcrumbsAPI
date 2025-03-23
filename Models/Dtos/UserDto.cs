namespace BreadcrumbsAPI.Models.Dtos;

public class UserDto
{
    public Guid? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    public string? Token { get; set; }
}

