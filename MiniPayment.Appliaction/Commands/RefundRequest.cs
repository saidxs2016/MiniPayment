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

public class RefundHandler : IRequestHandler<RefundRequest, TransactionDto>
{

    private readonly ITransactionRepository _transactionRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public RefundHandler(IServiceProvider serviceProvider, IMapper mapper, ITransactionRepository transactionRepository)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto> Handle(RefundRequest request, CancellationToken cancellationToken)
    {

        var transactionEntity = await _transactionRepository.GetAsync(i => i.OrderReference == request.OrderReference);
        if (transactionEntity.Id == Guid.Empty)
            throw new Exception("Ödemeniz bulunmadı.");

        var validTransactionToRefund = await _transactionRepository.GetAsync(i => i.OrderReference == request.OrderReference
                                        && i.TransactionDate > DateTime.Now.AddDays(-1));
        if (validTransactionToRefund.Id != Guid.Empty)
            throw new Exception("Ödemenize 24 saat geçtikten sonra para iadesi talebinde bulunabilirsiniz, devam etmek istiyorsanız iptal talebinde bulunabilirsiniz. ");

        if (transactionEntity.TransactionDetails[0].TransactionType != TransactionTypesHelper.Sale)
            throw new Exception("Para iade işlemi daha önce gerçekleştirilmiştir.");




        IBank bank;

        if (transactionEntity.BankId == BanksHelper.AKBANK)
            bank = _serviceProvider.GetService<IAkBankService>()!;
        else if (transactionEntity.BankId == BanksHelper.GARANTI)
            bank = _serviceProvider.GetService<IGarantiBankService>()!;
        else // YapiKrediBank
            bank = _serviceProvider.GetService<IYapiKrediBankService>()!;


        var transactionResponse = await bank.Refund(_mapper.Map<RefundTransaction>(request));
        return _mapper.Map<TransactionDto>(transactionResponse);
    }

}

public class RefundRequest : IRequest<TransactionDto>
{
    [DefaultValue("")]
    public string? OrderReference { get; set; }

}

/// <summary>
/// Fulent Validation ile Validasyon işlemi
/// </summary>
public class RefundValidator : AbstractValidator<RefundRequest>
{
    public RefundValidator()
    {
        RuleFor(p => p.OrderReference)
            .NotNull().WithMessage("Ödemeniz bulunmadı.")
            .NotEmpty().WithMessage("Ödemeniz bulunmadı.");
    }
}

