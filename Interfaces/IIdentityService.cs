namespace BreadcrumbsAPI.Interfaces;

public interface IIdentityService
{
    Task<Result> AddToRolesAsync(string email, List<string> roles);

    Task<bool> IsInRoleAsync(string email, string role);

    Task<UserDto?> AuthenticateAsync(string email, string password);

    Task<User> CreateUserAsync(RegisterDto registerDto);
}

public interface IJwtUtils
{
    string GenerateToken(string userId, string email, IList<string> roles);

    List<string> ValidateToken(string token);
}
