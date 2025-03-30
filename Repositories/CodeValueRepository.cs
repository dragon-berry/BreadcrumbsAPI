namespace BreadcrumbsAPI.Repositories;

public class CodeValueRepository : ICodeValueRepository
{
    private readonly BreadcrumbsDbContext context;

    public CodeValueRepository(BreadcrumbsDbContext _context)
    {
        context = _context;
    }

    public async Task<List<CodeValueDto>> GetCodeValues()
    {
        try
        {
            var codeValues = await context.CodeValues
                .Where(p => !p.IsDeleted)
                .ToListAsync();
            return codeValues.Adapt<List<CodeValueDto>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<List<CodeValueDto>> GetCodeValuesByGroupName(string groupName)
    {
        try
        {
            var codeValues = await context.CodeValues
                .Where(p => p.GroupName == groupName && !p.IsDeleted)
                .ToListAsync();
            return codeValues.Adapt<List<CodeValueDto>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
