namespace BreadcrumbsAPI.Applications.Commands;

public class RegisterCommand : IRequest<UserDto>
{
    public required RegisterDto RegisterDto { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
{
    private readonly IIdentityService identityService;
    private readonly IJwtUtils jwtUtils;

    public RegisterCommandHandler(IIdentityService _identityService, IJwtUtils _jwtUtils)
    {
        identityService = _identityService;
        jwtUtils = _jwtUtils;
    }

    public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await identityService.CreateUserAsync(request.RegisterDto);

            if (result is null)
                throw new Exception($"Unable to create {request.RegisterDto.Email}");

            var defaultRoles = new List<string> { "User" };

            // FOR TESTING PURPOSES ONLY - IF YOU INCLUDE ADMIN IN YOUR EMAIL ADDRESS, YOU WILL BE ASSIGNED THE ADMINISTRATOR ROLE
            if (request.RegisterDto.Email!.Contains("admin", StringComparison.OrdinalIgnoreCase))
                defaultRoles.Add("Administrator");

            if (result.Email == null)
                throw new Exception($"Unable to create {request.RegisterDto.Email}");

            var addUserToRole = await identityService.AddToRolesAsync(result.Email, defaultRoles);
            if (addUserToRole == null)
            {
                var errors = string.Join(Environment.NewLine, addUserToRole!.Errors);
                throw new Exception($"Unable to add {request.RegisterDto.Email} to assigned role/s.{Environment.NewLine}{errors}");
            }

            var userDto = result.Adapt<UserDto>();
            userDto.Token = jwtUtils.GenerateToken(result.Id, result.Email, defaultRoles);

            return userDto;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
