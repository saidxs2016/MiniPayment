using AutoMapper;
using MiniPayment.Appliaction.Commands;
using MiniPayment.Domain.Entities;
using MiniPayment.Domain.Helpers;
using MiniPayment.Domain.TransactionsModel;
using System.Globalization;

namespace MiniPayment.Appliaction.DTO.Mapper;

public partial class GlobalMapper : Profile
{
    readonly CultureInfo turkishCulture = new("tr-TR");

    public GlobalMapper()
    {
        CreateMap<SaleRequest, SaleTransaction>()
            .ForMember(dest => dest.BankId, j => j.MapFrom(src => src.BankId))
            .ForMember(dest => dest.Status, j => j.MapFrom(src => false))
            .ForMember(dest => dest.OrderReference, j => j.MapFrom(src => ""))
            .ForMember(dest => dest.TotalAmount, j => j.MapFrom(src => src.Amount))
            .ForMember(dest => dest.NetAmount, j => j.MapFrom(src => src.Amount))
            .ForMember(dest => dest.TransactionDate, j => j.MapFrom(src => DateTime.Now));

        CreateMap<CancelRequest, CancelTransaction>()
            .ForMember(dest => dest.Status, j => j.MapFrom(src => false))
            .ForMember(dest => dest.OrderReference, j => j.MapFrom(src => src.OrderReference))
            .ForMember(dest => dest.TransactionDate, j => j.MapFrom(src => DateTime.Now));

        CreateMap<RefundRequest, RefundTransaction>()
            .ForMember(dest => dest.Status, j => j.MapFrom(src => false))
            .ForMember(dest => dest.OrderReference, j => j.MapFrom(src => src.OrderReference))
            .ForMember(dest => dest.TransactionDate, j => j.MapFrom(src => DateTime.Now));

        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.BankId, j => j.MapFrom(src => src.BankId))
            .ForMember(dest => dest.Bank, j => j.MapFrom(src => GetBankAsString(src)))
            .ForMember(dest => dest.OrderReferance, j => j.MapFrom(src => src.OrderReference))
            .ForMember(dest => dest.Amount, j => j.MapFrom(src => src.TotalAmount.ToString("C", turkishCulture)))
            .ForMember(dest => dest.TransactionType, j => j.MapFrom(src => GetTransactionTypeAsString(src)))
            .ForMember(dest => dest.TransactionTypeStatu, j => j.MapFrom(src => GetTransactionTypeStatuAsString(src)));


        CreateMap<Transaction, ReportDto>()
            .ForMember(dest => dest.BankId, j => j.MapFrom(src => src.BankId))
            .ForMember(dest => dest.Bank, j => j.MapFrom(src => GetBankAsString(src)))
            .ForMember(dest => dest.OrderReferance, j => j.MapFrom(src => src.OrderReference))
            .ForMember(dest => dest.Status, j => j.MapFrom(src => GetStatusAsString(src)))
            .ForMember(dest => dest.TotalAmount, j => j.MapFrom(src => src.TotalAmount.ToString("C", turkishCulture)))
            .ForMember(dest => dest.NetAmount, j => j.MapFrom(src => src.NetAmount.ToString("C", turkishCulture)))
            .ForMember(dest => dest.TransactionDate, j => j.MapFrom(src => src.TransactionDate.ToString("F", turkishCulture)))
            .ForMember(dest => dest.TransactionType, j => j.MapFrom(src => GetTransactionTypeAsString(src)))
            .ForMember(dest => dest.TransactionTypeStatu, j => j.MapFrom(src => GetTransactionTypeStatuAsString(src)));
    }

    private string GetBankAsString(Transaction transaction)
    {
        if (transaction == null)
            return "";
        else if (transaction.BankId == BanksHelper.AKBANK)
            return "AKBANK";
        else if (transaction.BankId == BanksHelper.GARANTI)
            return "GARANTİ";
        else if (transaction.BankId == BanksHelper.YAPIKREDI)
            return "YAPİ KREDİ";
        else
            return "";
    }
    private string GetStatusAsString(Transaction transaction)
    {
        if (transaction == null)
            return "";
        else if (transaction.Status == StatusHelper.SUCCESS)
            return "İşlem Başarılı";
        else
            return "İşlem Başarısız";
    }
    private string GetTransactionTypeAsString(Transaction transaction)
    {
        if (transaction == null || transaction.TransactionDetails == null || transaction.TransactionDetails.Count == 0)
            return "";

        return transaction.TransactionDetails[0].TransactionType;
    }
    private string GetTransactionTypeStatuAsString(Transaction transaction)
    {
        if (transaction == null || transaction.TransactionDetails == null || transaction.TransactionDetails.Count == 0)
            return "";

        var statu = transaction.TransactionDetails[0].Status;

        if (statu == StatusHelper.SUCCESS)
            return "İşlem Başarılı";
        else
            return "İşlem Başarısız";
    }


}
