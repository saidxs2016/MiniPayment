using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiniPayment.Appliaction.DTO;
using MiniPayment.Appliaction.Interfaces;
using MiniPayment.Appliaction.Interfaces.BanksInterfaces;
using MiniPayment.Appliaction.Interfaces.PersistenceRepositories;
using MiniPayment.Domain.Helpers;
using MiniPayment.Domain.TransactionsModel;
using System.ComponentModel;

namespace MiniPayment.Appliaction.Commands;

public class CancelHandler : IRequestHandler<CancelRequest, TransactionDto>
{

    private readonly ITransactionRepository _transactionRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public CancelHandler(IServiceProvider serviceProvider, IMapper mapper, ITransactionRepository transactionRepository)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto> Handle(CancelRequest request, CancellationToken cancellationToken)
    {

        var transactionEntity = await _transactionRepository.GetAsync(i => i.OrderReference == request.OrderReference);
        if (transactionEntity.Id == Guid.Empty)
            throw new Exception("Ödemeniz bulunmadı.");

        var validTransactionToCancel = await _transactionRepository.GetAsync(i => i.OrderReference == request.OrderReference && i.TransactionDate < DateTime.Now.AddDays(-1));
        if (validTransactionToCancel.Id != Guid.Empty)
            throw new Exception("İptal işleminiz artık gerçekleştirilmiyor.");

        if (transactionEntity.TransactionDetails[0].TransactionType != TransactionTypesHelper.Sale)
            throw new Exception("Bu ödeme daha önce iptal edildi.");





        IBank bank;

        if (transactionEntity.BankId == BanksHelper.AKBANK)
            bank = _serviceProvider.GetService<IAkBankService>()!;
        else if (transactionEntity.BankId == BanksHelper.GARANTI)
            bank = _serviceProvider.GetService<IGarantiBankService>()!;
        else // YapiKrediBank
            bank = _serviceProvider.GetService<IYapiKrediBankService>()!;


        var transactionResponse = await bank.Cancel(_mapper.Map<CancelTransaction>(request));

        return _mapper.Map<TransactionDto>(transactionResponse);
    }

}

public class CancelRequest : IRequest<TransactionDto>
{
    [DefaultValue("")]
    public string? OrderReference { get; set; }
}

/// <summary>
/// Fulent Validation ile Validasyon işlemi
/// </summary>
public class CancelValidator : AbstractValidator<CancelRequest>
{
    public CancelValidator()
    {
        RuleFor(p => p.OrderReference)
            .NotNull().WithMessage("Ödemeniz bulunmadı.")
            .NotEmpty().WithMessage("Ödemeniz bulunmadı.");
    }
}

