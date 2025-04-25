namespace BreadcrumbsAPI.Repositories;

public class CrumbRepository : ICrumbRepository
{
    private readonly BreadcrumbsDbContext context;

    public CrumbRepository(BreadcrumbsDbContext _context)
    {
        context = _context;
    }

    public async Task<List<CrumbDto>> GetCrumbs()
    {
        try
        {
            var crumbs = await context.Crumbs
                .Include(p => p.Location)
                .Include(p => p.CrumbTypeCv)
                .Include(p => p.Group)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            return crumbs.Adapt<List<CrumbDto>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<CrumbDto> AddCrumb(CrumbDto crumbDto)
    {
        try
        {
            var crumb = crumbDto.Adapt<Crumb>();
            await context.Crumbs.AddAsync(crumb);
            await context.SaveChangesAsync(new CancellationToken());
            return crumb.Adapt<CrumbDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public Task<bool> UpdateCrumb(CrumbDto crumbDto)
    {
        try
        {
            throw new NotImplementedException("UpdateCrumb method is not implemented yet. (we don't need this since you can't update a crumb)");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteCrumb(Guid id)
    {
        try
        {
            var crumb = await GetCrumb(context, id);

            crumb.IsDeleted = true;
            context.Crumbs.Update(crumb);

            await context.SaveChangesAsync(new CancellationToken());
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private async Task<Crumb> GetCrumb(BreadcrumbsDbContext context, Guid id)
    {
        return await context.Crumbs
            .Include(p => p.Location)
            .Include(p => p.CrumbTypeCv)
            .Include(p => p.Group)
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception();
    }
}
