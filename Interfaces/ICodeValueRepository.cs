namespace BreadcrumbsAPI.Interfaces;

public interface ICodeValueRepository
{
    Task<List<CodeValueDto>> GetCodeValues();
    Task<List<CodeValueDto>> GetCodeValuesByGroupName(string groupName);
}
