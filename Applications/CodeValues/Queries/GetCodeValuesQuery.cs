namespace BreadcrumbsAPI.Applications.CodeValues.Queries;

public class GetCodeValuesQuery : IRequest<List<CodeValueDto>>
{
}

public class GetCodeValuesQueryHandler : IRequestHandler<GetCodeValuesQuery, List<CodeValueDto>>
{
    private readonly ICodeValueRepository codeValuesRepository;
    public GetCodeValuesQueryHandler(ICodeValueRepository _codeValuesRepository)
    {
        codeValuesRepository = _codeValuesRepository;
    }

    public async Task<List<CodeValueDto>> Handle(GetCodeValuesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await codeValuesRepository.GetCodeValues();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}