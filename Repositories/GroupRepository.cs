namespace BreadcrumbsAPI.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly BreadcrumbsDbContext context;

    public GroupRepository(BreadcrumbsDbContext _context)
    {
        context = _context;
    }

    public async Task<List<GroupDto>> GetGroups(Guid userId)
    {
        try
        {
            var Groups = await context.GroupUserRelationships
                .Where(p => !p.IsDeleted && p.UserId == userId && p.StatusCvId == CodeValueConstants.ActiveStatus)
                .Include(p => p.Group)
                .Select(p => p.Group)
                .ToListAsync();
            return Groups.Adapt<List<GroupDto>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<List<GroupDto>> GetAllGroups()
    {
        try
        {
            var Groups = await context.Groups
                .Include(p => p.GroupUserRelationships)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
            return Groups.Adapt<List<GroupDto>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<GroupDto> AddGroup(GroupDto groupDto, Guid userId)
    {
        try
        {
            groupDto.Code = GenerateRandomCode();

            //TODO: Have this populated in the frontend
            if (groupDto.LifeSpanCvId is null)
                groupDto.LifeSpanCvId = CodeValueConstants.OneDayLifeSpan;

            var group = groupDto.Adapt<Group>();
            group.GroupUserRelationships = new List<GroupUserRelationship>
            {
                new GroupUserRelationship
                {
                    UserId = userId,
                    IsOwner = true,
                    StatusCvId = CodeValueConstants.ActiveStatus
                }
            };

            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync(new CancellationToken());
            return group.Adapt<GroupDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateGroup(GroupDto groupDto)
    {
        try
        {
            var group = await context.Groups.FirstOrDefaultAsync(p => p.Id == groupDto.Id);
            group = groupDto.Adapt<Group>();
            context.Groups.Update(group);
            await context.SaveChangesAsync(new CancellationToken());
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> AddUserToGroup(string groupCode, Guid userId)
    {
        try
        {
            var group = await context.Groups.FirstOrDefaultAsync(p => p.Code == groupCode);
            if (group != null)
            {
                var user = await context.Users.FirstOrDefaultAsync(p => p.Id == userId);
                if (user != null)
                {
                    var groupUserRelationship = new GroupUserRelationship
                    {
                        GroupId = group.Id,
                        UserId = user.Id,
                        StatusCvId = CodeValueConstants.ActiveStatus
                    };
                    await context.GroupUserRelationships.AddAsync(groupUserRelationship);
                    await context.SaveChangesAsync(new CancellationToken());
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> LeaveGroup(Guid groupId, Guid userId)
    {
        try
        {
            var group = await context.Groups.FirstOrDefaultAsync(p => p.Id == groupId);
            if (group != null)
            {
                var user = await context.Users.FirstOrDefaultAsync(p => p.Id == userId);
                if (user != null)
                {
                    var groupUserRelationship = await context.GroupUserRelationships.FirstOrDefaultAsync(p => p.GroupId == group.Id && p.UserId == user.Id);
                    if (groupUserRelationship != null)
                    {
                        groupUserRelationship.IsDeleted = true;
                        groupUserRelationship.StatusCvId = CodeValueConstants.RemovedStatus;
                        context.Groups.Update(group);
                        await context.SaveChangesAsync(new CancellationToken());
                        return true;
                    }
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteGroup(Guid groupId)
    {
        try
        {
            var group = await context.Groups.Include(g => g.GroupUserRelationships).FirstOrDefaultAsync(p => p.Id == groupId);
            if (group != null)
            {
                if(group.GroupUserRelationships != null && group.GroupUserRelationships.Count > 1)
                {
                    group.GroupUserRelationships.ToList().ForEach(gur =>
                    {
                        gur.IsDeleted = true;
                        gur.StatusCvId = CodeValueConstants.RemovedStatus;
                    });
                }

                group.IsDeleted = true;
                context.Groups.Update(group);
                await context.SaveChangesAsync(new CancellationToken());
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private static string GenerateRandomCode(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
