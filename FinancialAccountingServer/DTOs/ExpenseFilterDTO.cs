namespace FinancialAccountingServer.DTOs
{
    public class ExpenseFilterDTO
    {
        public string? Amount { get; set; }
        public string? Date {  get; set; }
        public int? CategoryId { get; set; }
    }
}
