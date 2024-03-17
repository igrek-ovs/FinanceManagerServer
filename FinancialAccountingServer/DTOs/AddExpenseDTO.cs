namespace FinancialAccountingServer.DTOs
{
    public class AddExpenseDTO
    {
        public string Description { get; set; }

        public double Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public int CategoryId { get; set; }
    }
}
