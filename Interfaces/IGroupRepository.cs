namespace BreadcrumbsAPI.Interfaces;

public interface IGroupRepository
{
    Task<List<GroupDto>> GetGroups(Guid userId);
    Task<List<GroupDto>> GetAllGroups();
    Task<GroupDto> AddGroup(GroupDto groupDto, Guid userId);
    Task<bool> AddUserToGroup(string groupCode, Guid userId);
    Task<bool> UpdateGroup(GroupDto groupDto);
    Task<bool> LeaveGroup(Guid groupId, Guid userId);
    Task<bool> DeleteGroup(Guid groupId);
}
