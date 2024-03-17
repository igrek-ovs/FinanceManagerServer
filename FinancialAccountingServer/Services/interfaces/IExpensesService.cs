using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;

namespace FinancialAccountingServer.Services.interfaces
{
    public interface IExpensesService
    {
        Task<List<ExpenseDTO>> GetAllExpenses();

        Task<List<ExpenseDTO>> GetAllExpensesForUser(int groupId, int userId);

        Task<List<ExpenseDTO>> GetAllExpensesForGroup(int groupId);

        Task<Dictionary<string, List<ExpenseDTO>>> GetAllExpensesGroupedByUser(int groupId);

        Task<List<ExpenseDTO>> GetExpensesForPeriodOfTime(int groupId, DateTime startDate, DateTime endDate);

        Task<bool> UpdateCategoryOfExpense(int expenseId, int newCategoryId);

        Task<bool> UpdateDescriptionOfExpense(int expenseId, string newDescription);

        Task<bool> UpdateAmountOfExpense(int expenseId, double newAmount);

        Task<bool> RemoveExpense(int expenseId);

        Task<ExpenseDTO> AddExpense(AddExpenseDTO expense);

        Task<bool> CanUserModifyExpense(int userId, int expenseId);

        Task<bool> IsUserGroupAdmin(int userId, int groupId);
    }
}
