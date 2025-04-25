namespace BreadcrumbsAPI.Helpers;

public static class ClaimsHelper
{
    public static Guid GetUserId(IHttpContextAccessor accessor)
    {
        var userIdClaim = accessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? accessor?.HttpContext?.User?.FindFirst("UserId")?.Value;
        if (!string.IsNullOrWhiteSpace(userIdClaim) && Guid.TryParse(userIdClaim, out Guid userId))
            return userId;

        throw new Exception("UserId was not found or is invalid.");
    }
}