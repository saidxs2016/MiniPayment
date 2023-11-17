using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiniPayment.Appliaction.DTO;
using MiniPayment.Appliaction.Interfaces;
using MiniPayment.Appliaction.Interfaces.BanksInterfaces;
using MiniPayment.Domain.Helpers;
using MiniPayment.Domain.TransactionsModel;
using System.ComponentModel;

namespace MiniPayment.Appliaction.Commands;

public class SaleHandler : IRequestHandler<SaleRequest, TransactionDto>
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public SaleHandler(IServiceProvider serviceProvider, IMapper mapper)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(SaleRequest request, CancellationToken cancellationToken)
    {
        // ============ Factory Design Pattern ============
        IBank bank;

        if (request.BankId == BanksHelper.AKBANK)
            bank = _serviceProvider.GetService<IAkBankService>()!;
        else if (request.BankId == BanksHelper.GARANTI)
            bank = _serviceProvider.GetService<IGarantiBankService>()!;
        else // YapiKrediBank
            bank = _serviceProvider.GetService<IYapiKrediBankService>()!;




        var transactionResponse = await bank.Pay(_mapper.Map<SaleTransaction>(request));
        return _mapper.Map<TransactionDto>(transactionResponse);
    }

}

public class SaleRequest : IRequest<TransactionDto>
{
    [DefaultValue("4355084355084358")]
    public string? CardNumber { get; set; }

    [DefaultValue("Said YUNUS")]
    public string? CardHolderName { get; set; }

    [DefaultValue("11/27")]
    public string? ExpirationDate { get; set; }

    [DefaultValue("123")]
    public string? Cvv { get; set; }

    public decimal Amount { get; set; }
    public short BankId { get; set; }

}

/// <summary>
/// Fulent Validation ile Validasyon işlemi
/// </summary>
public class SaleValidator : AbstractValidator<SaleRequest>
{
    public SaleValidator()
    {       
        RuleFor(p => p.Amount)
            .Must(i => i > 0).WithMessage("Kartınızın bakiyesi bitmiştir.");

        RuleFor(p => p.BankId)
            .Must(i => new List<short>() { BanksHelper.AKBANK, BanksHelper.GARANTI, BanksHelper.YAPIKREDI }.Contains(i)).WithMessage("Şu an sadece Akbank, Garanti veya YapıKredi kartlarıyla ödeme yapılabilmektedir. ");
    }
}
