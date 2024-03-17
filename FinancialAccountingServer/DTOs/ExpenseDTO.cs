namespace FinancialAccountingServer.DTOs
{
    public class ExpenseDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
