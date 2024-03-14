namespace FinancialAccountingServer.models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int CategoryId { get; set; }
        public ExpenseCategory Category { get; set; }
    }
}
