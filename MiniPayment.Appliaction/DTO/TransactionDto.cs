namespace MiniPayment.Appliaction.DTO;

public class TransactionDto
{
    public short BankId { get; set; }
    public string? Bank { get; set; }
    public string? Amount { get; set; }
    public string? OrderReferance { get; set; }
    public string? TransactionType { get; set; }
    public string? TransactionTypeStatu { get; set; }
}
