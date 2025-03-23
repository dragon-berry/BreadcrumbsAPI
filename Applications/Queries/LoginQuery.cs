namespace BreadcrumbsAPI.Applications.Queries;

public class LoginQuery : IRequest<UserDto?>
{
    public required LoginDto LoginDto { get; set; }
}

public class LoginQueryHandler : IRequestHandler<LoginQuery, UserDto?>
{
    private readonly IIdentityService identityService;
    private readonly IJwtUtils jwtUtils;

    public LoginQueryHandler(IIdentityService _identityService, IJwtUtils _jwtUtils)
    {
        identityService = _identityService;
        jwtUtils = _jwtUtils;
    }

    public async Task<UserDto?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await identityService.AuthenticateAsync(request.LoginDto.Email!, request.LoginDto.Password!);
            if (user is null)
                return null;

            var defaultRoles = new List<string> { "User" };
            user.Token = jwtUtils.GenerateToken(user.Id.ToString()!.ToLower(), user.Email!, user.Roles != null ? user.Roles : defaultRoles);
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
