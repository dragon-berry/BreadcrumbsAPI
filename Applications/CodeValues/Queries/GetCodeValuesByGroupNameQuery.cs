namespace BreadcrumbsAPI.Applications.CodeValues.Queries;

public class GetCodeValuesByGroupNameQuery : IRequest<List<CodeValueDto>>
{
    public required string GroupName { get; set; }
}

public class GetCodeValuesByGroupNameQueryHandler : IRequestHandler<GetCodeValuesByGroupNameQuery, List<CodeValueDto>>
{
    private readonly ICodeValueRepository codeValuesRepository;
    public GetCodeValuesByGroupNameQueryHandler(ICodeValueRepository _codeValuesRepository)
    {
        codeValuesRepository = _codeValuesRepository;
    }

    public async Task<List<CodeValueDto>> Handle(GetCodeValuesByGroupNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await codeValuesRepository.GetCodeValuesByGroupName(request.GroupName);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}