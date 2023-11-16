using AutoMapper;
using FluentValidation;
using MediatR;
using MiniPayment.Appliaction.DTO;
using MiniPayment.Appliaction.Interfaces.PersistenceRepositories;
using System.ComponentModel;

namespace MiniPayment.Appliaction.Queries;

public class FilterByHandler : IRequestHandler<FilterByRequest, List<ReportDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    public FilterByHandler(ITransactionRepository transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<List<ReportDto>> Handle(FilterByRequest request, CancellationToken cancellationToken)
    {
        var enities = await _transactionRepository.Filter(request);
        return _mapper.Map<List<ReportDto>>(enities);
    }

}

public class FilterByRequest: IRequest<List<ReportDto>>
{    
    [DefaultValue("2023-01-01T00:00:00")]
    public DateTime? StartDate { get; set; }

    [DefaultValue(null)]
    public DateTime? EndDate { get; set; }

    [DefaultValue("")]
    public string? OrderReference { get; set; }


    public short? BankId { get; set; }

    [DefaultValue(null)]
    public bool? Status { get; set; }
}



/// <summary>
/// Fulent Validation ile Validasyon işlemi
/// </summary>
public class FilterByValidator : AbstractValidator<FilterByRequest>
{
    public FilterByValidator()
    {

    }

}
