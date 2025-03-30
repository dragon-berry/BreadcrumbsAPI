namespace BreadcrumbsAPI.Interfaces;

public interface ICrumbRepository
{
    Task<List<CrumbDto>> GetCrumbs();
    Task<CrumbDto> AddCrumb(CrumbDto crumbDto);
    Task<bool> UpdateCrumb(CrumbDto crumbDto);
    Task<bool> DeleteCrumb(Guid crumbId);
}
