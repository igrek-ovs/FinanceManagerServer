namespace FinancialAccountingServer.models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public int UserId { get; set; }
        //public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
