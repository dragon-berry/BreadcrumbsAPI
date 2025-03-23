namespace BreadcrumbsAPI.Data;

public class BreadcrumbsDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public BreadcrumbsDbContext(DbContextOptions<BreadcrumbsDbContext> options, IHttpContextAccessor? httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    //Database Tables
    public DbSet<CodeValue> CodeValues { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUserRelationship> GroupUserRelationships { get; set; }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value;

        Guid userId = SystemConstants.SystemUserId;
        if (!string.IsNullOrWhiteSpace(userIdClaim))
        {
            Guid.TryParse(userIdClaim, out userId);
        }

        var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                var now = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entity.CreationDate = now;
                    entity.CreationUserId = userId;
                }

                entity.UpdatedDate = now;
                entity.UpdatedUserId = userId;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
