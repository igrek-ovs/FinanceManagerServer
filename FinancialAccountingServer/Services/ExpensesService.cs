using FinancialAccountingServer.Data;
using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;
using FinancialAccountingServer.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountingServer.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly FinancialAppContext _financialAppContext;

        public ExpensesService(FinancialAppContext financialAppContext)
        {
            _financialAppContext = financialAppContext;
        }

        public async Task<ExpenseDTO> AddExpense(AddExpenseDTO expense)
        {
            //bool canAddExpense = await CanUserModifyExpense(expense.UserId, expense.GroupId);
            //if (!canAddExpense)
            //{
            //    throw new InvalidOperationException("User does not have permission to add expenses to this group.");
            //}

            var newExpense = new Expense
            {
                Description = expense.Description,
                Amount = expense.Amount,
                CreatedAt = expense.CreatedAt,
                UserId = expense.UserId,
                GroupId = expense.GroupId,
                CategoryId = expense.CategoryId
            };

            _financialAppContext.Expenses.Add(newExpense);
            await _financialAppContext.SaveChangesAsync();

            return new ExpenseDTO
            {
                Id = newExpense.Id,
                Description = newExpense.Description,
                Amount = newExpense.Amount,
                CreatedAt = newExpense.CreatedAt,
                UserId = newExpense.UserId,
                CategoryId = newExpense.CategoryId,
            };
        }

        public async Task<bool> CanUserModifyExpense(int userId, int expenseId)
        {
            var expense = await _financialAppContext.Expenses
                .Include(exp => exp.User)
                .FirstOrDefaultAsync(exp => exp.Id == expenseId);

            if (expense == null)
            {
                return false;
            }

            return expense.User.Id == userId || await IsUserGroupAdmin(userId, expense.GroupId);
        }

        public async Task<bool> IsUserGroupAdmin(int userId, int groupId)
        {
            var groupMembership = await _financialAppContext.GroupMembers
                .FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId == groupId);

            return groupMembership != null && groupMembership.IsAdmin;
        }

        public async Task<List<ExpenseDTO>> GetAllExpenses()
        {
            return await _financialAppContext.Expenses
                .Include(exp => exp.User)
                .Include(exp => exp.Category)
                .Select(exp => new ExpenseDTO
                {
                    Id = exp.Id,
                    Description = exp.Description,
                    Amount = exp.Amount,
                    CreatedAt = exp.CreatedAt,
                    UserId = exp.UserId,
                    UserName = exp.User.Username,
                    CategoryId = exp.CategoryId,
                    CategoryName = exp.Category.Name
                })
                .ToListAsync();
        }

        public async Task<List<ExpenseDTO>> GetAllExpensesForGroup(int groupId)
        {
            return await _financialAppContext.Expenses
                .Include(exp => exp.User)
                .Include(exp => exp.Category)
                .Where(exp => exp.GroupId == groupId)
                .Select(exp => new ExpenseDTO
                {
                    Id = exp.Id,
                    Description = exp.Description,
                    Amount = exp.Amount,
                    CreatedAt = exp.CreatedAt,
                    UserId = exp.UserId,
                    UserName = exp.User.Username,
                    CategoryId = exp.CategoryId,
                    CategoryName = exp.Category.Name
                })
                .ToListAsync();
        }

        public async Task<List<ExpenseDTO>> GetAllExpensesForUser(int groupId, int userId)
        {
            return await _financialAppContext.Expenses
                .Include(exp => exp.User)
                .Include(exp => exp.Category)
                .Where(exp => exp.GroupId == groupId && exp.User.Id == userId)
                .Select(exp => new ExpenseDTO
                {
                    Id = exp.Id,
                    Description = exp.Description,
                    Amount = exp.Amount,
                    CreatedAt = exp.CreatedAt,
                    UserId = exp.UserId,
                    UserName = exp.User.Username,
                    CategoryId = exp.CategoryId,
                    CategoryName = exp.Category.Name
                })
                .ToListAsync();
        }

        public async Task<Dictionary<string, List<ExpenseDTO>>> GetAllExpensesGroupedByUser(int groupId)
        {
            var expensesGroupedByUser = await _financialAppContext.Expenses
                .Include(exp => exp.User)
                .Include(exp => exp.Category)
                .Where(exp => exp.GroupId == groupId)
                .GroupBy(exp => exp.User.Username)
                .ToDictionaryAsync(
                    group => group.Key,
                    group => group.Select(exp => new ExpenseDTO
                    {
                        Id = exp.Id,
                        Description = exp.Description,
                        Amount = exp.Amount,
                        CreatedAt = exp.CreatedAt,
                        UserId = exp.UserId,
                        UserName = exp.User.Username,
                        CategoryId = exp.CategoryId,
                        CategoryName = exp.Category.Name
                    }).ToList());

            return expensesGroupedByUser;
        }

        public async Task<List<ExpenseDTO>> GetExpensesForPeriodOfTime(int groupId, DateTime startDate, DateTime endDate)
        {
            var expensesForPeriod = await _financialAppContext.Expenses
                .Include(exp => exp.User)
                .Include(exp => exp.Category)
                .Where(exp => exp.GroupId == groupId && exp.CreatedAt >= startDate && exp.CreatedAt <= endDate)
                .Select(exp => new ExpenseDTO
                {
                    Id = exp.Id,
                    Description = exp.Description,
                    Amount = exp.Amount,
                    CreatedAt = exp.CreatedAt,
                    UserId = exp.UserId,
                    UserName = exp.User.Username,
                    CategoryId = exp.CategoryId,
                    CategoryName = exp.Category.Name
                })
                .ToListAsync();

            return expensesForPeriod;
        }

        public async Task<bool> RemoveExpense(int expenseId)
        {
            var expense = await _financialAppContext.Expenses.FirstOrDefaultAsync(exp => exp.Id == expenseId);
            if(expense == null)
            {
                return false;
            }
            _financialAppContext.Expenses.Remove(expense);
            await _financialAppContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAmountOfExpense(int expenseId, double newAmount)
        {
            var expense = await _financialAppContext.Expenses.FirstOrDefaultAsync(exp => exp.Id == expenseId);
            if (expense == null)
            {
                return false;
            }

            expense.Amount = newAmount;
            await _financialAppContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCategoryOfExpense(int expenseId, int newCategoryId)
        {
            var expense = await _financialAppContext.Expenses.FirstOrDefaultAsync(exp => exp.Id == expenseId);
            if (expense == null)
            {
                return false;
            }

            var categoryExists = await _financialAppContext.Expenses.AnyAsync(cat => cat.Category.Id == newCategoryId);
            if (!categoryExists)
            {
                return false;
            }

            expense.CategoryId = newCategoryId;
            await _financialAppContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateDescriptionOfExpense(int expenseId, string newDescription)
        {
            var expense = await _financialAppContext.Expenses.FirstOrDefaultAsync(exp => exp.Id == expenseId);
            if (expense == null)
            {
                return false;
            }

            expense.Description = newDescription;
            await _financialAppContext.SaveChangesAsync();

            return true;
        }


    }
}
