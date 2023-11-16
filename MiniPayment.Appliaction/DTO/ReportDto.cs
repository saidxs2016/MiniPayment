namespace MiniPayment.Appliaction.DTO;

public class ReportDto
{
    public short BankId { get; set; }
    public string? Bank { get; set; }
    public string? TotalAmount { get; set; }
    public string? NetAmount { get; set; }
    public string? Status { get; set; }
    public string? OrderReferance { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? TransactionType { get; set; }
    public string? TransactionTypeStatu { get; set; }
}
