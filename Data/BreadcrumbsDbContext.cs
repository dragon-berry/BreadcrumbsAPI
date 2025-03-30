namespace BreadcrumbsAPI.Data;

public class BreadcrumbsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public BreadcrumbsDbContext(DbContextOptions<BreadcrumbsDbContext> options, IHttpContextAccessor? httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    //Database Tables
    public DbSet<CodeValue> CodeValues { get; set; }
    public DbSet<Crumb> Crumbs { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUserRelationship> GroupUserRelationships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Crumb Model
        modelBuilder.Entity<Crumb>()
            .HasOne(c => c.LifeSpanCv)
            .WithMany()
            .HasForeignKey(c => c.LifeSpanCvId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Crumb>()
            .HasOne(c => c.Location)
            .WithMany()
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Crumb>()
            .HasOne(c => c.CrumbTypeCv)
            .WithMany()
            .HasForeignKey(c => c.CrumbTypeCvId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Crumb>()
            .HasOne(c => c.Group)
            .WithMany()
            .HasForeignKey(c => c.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        //Group Model
        modelBuilder.Entity<Group>()
            .HasOne(g => g.LifeSpanCv)
            .WithMany()
            .HasForeignKey(g => g.LifeSpanCvId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Group>().HasIndex(g => g.Code).IsUnique();

        //GroupUserRelationship Model
        modelBuilder.Entity<GroupUserRelationship>()
            .HasOne(gur => gur.StatusCv)
            .WithMany()
            .HasForeignKey(gur => gur.StatusCvId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupUserRelationship>()
            .HasOne(gur => gur.Group)
            .WithMany(g => g.GroupUserRelationships)
            .HasForeignKey(gur => gur.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupUserRelationship>()
            .HasOne(gur => gur.User)
            .WithMany(u => u.GroupUserRelationships)
            .HasForeignKey(gur => gur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupUserRelationship>().HasIndex(gur => new { gur.GroupId, gur.UserId }).IsUnique();

        //Location Model
        modelBuilder.Entity<Location>()
            .HasOne(l => l.LocationTypeCv)
            .WithMany()
            .HasForeignKey(l => l.LocationTypeCvId)
            .OnDelete(DeleteBehavior.Restrict);
    }

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
