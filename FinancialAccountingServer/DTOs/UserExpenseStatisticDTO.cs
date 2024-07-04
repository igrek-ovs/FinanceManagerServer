namespace FinancialAccountingServer.DTOs
{
    public class UserExpenseStatisticDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double LastMonthExpenses { get; set; }
        public double LastWeekExpenses { get; set; }
        public double AvgDayExpenses { get; set; }
        public double TotalExpenses { get; set; }
        public string MostPopularCategory {  get; set; }
    }
}
