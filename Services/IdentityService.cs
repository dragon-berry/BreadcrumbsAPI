namespace BreadcrumbsAPI.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public IdentityService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<User> CreateUserAsync(RegisterDto registerDto)
    {
        var user = registerDto.Adapt<User>();
        user.UserName = registerDto.Email;
        var result = await _userManager.CreateAsync(user, registerDto.Password!);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return user;
    }

    public async Task<bool> UserExists(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<bool> IsInRoleAsync(string email, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Email == email);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<UserDto?> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return null;

        var isCorrect = await _userManager.CheckPasswordAsync(user, password);
        if (!isCorrect)
            return null;

        var userDto = user.Adapt<UserDto>();

        //Get user roles
        var roles = await _userManager.GetRolesAsync(user);
        userDto.Roles = roles.ToList();
        return userDto;
    }

    public async Task<Result> AddToRolesAsync(string email, List<string> roles)
    {
        var administratorRole = new IdentityRole<Guid>(RoleConstants.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        var supplierRole = new IdentityRole<Guid>(RoleConstants.Supplier);
        if (_roleManager.Roles.All(r => r.Name != supplierRole.Name))
        {
            await _roleManager.CreateAsync(supplierRole);
        }

        var user = await _userManager.Users.FirstAsync(u => u.Email == email);
        if (user == null)
        {
            return Result.Failure(new List<string> { "User not found" });
        }

        foreach (var role in roles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var result = await _userManager.AddToRolesAsync(user, roles);
        if (result.Succeeded)
        {
            return Result.Success();
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description);
            return Result.Failure(errors);
        }
    }
}

public class JwtUtils : IJwtUtils
{
    private readonly IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string userId, string email, IList<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtOptions");
        Guard.Against.Null(jwtSettings, message: "JwtOptions not found.");

        var key = Guard.Against.NullOrEmpty(jwtSettings["Secret"], message: "'Secret' not found or empty.");
        var issuer = Guard.Against.NullOrEmpty(jwtSettings["Issuer"], message: "'Issuer' not found or empty.");
        var audience = Guard.Against.NullOrEmpty(jwtSettings["Audience"], message: "'Audience' not found or empty.");
        var expiryMinutes = Guard.Against.NullOrEmpty(jwtSettings["expiryInMinutes"], message: "'expiryInMinutes' not found or empty.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, userId),
            new Claim("UserId", userId),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(expiryMinutes)),
            signingCredentials: signingCredentials
            );

        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

        return encodedToken;
    }

    public List<string> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSettings = _configuration.GetSection("JwtOptions");
        Guard.Against.Null(jwtSettings, message: "JwtOptions not found.");
        var key = Guard.Against.NullOrEmpty(jwtSettings["Secret"], message: "'Secret' not found or empty.");

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,

            ClockSkew = TimeSpan.FromMinutes(20)
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        if (jwtToken != null)
        {
            var roles = new List<string>();
            foreach (var claim in jwtToken.Claims)
            {
                if (claim.Type.ToLower() == "role")
                {
                    roles.Add(claim.Value);
                }
            }
            return roles;
        }

        return new List<string>();
    }
}
