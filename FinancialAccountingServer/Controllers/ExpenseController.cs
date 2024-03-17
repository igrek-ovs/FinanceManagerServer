using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAccountingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpensesService _expensesService;

        public ExpenseController(IExpensesService expensesService) 
        {
            _expensesService = expensesService;
        }

        [HttpGet("get-all-expenses")]
        public async Task<ActionResult<List<ExpenseDTO>>> GetAllExpenses()
        {
            var expenses = await _expensesService.GetAllExpenses();
            return Ok(expenses);
        }

        [HttpGet("get-all-expenses-for-group")]
        public async Task<ActionResult<List<ExpenseDTO>>> GetAllExpensesForGroup(int groupId)
        {
            var expenses = await _expensesService.GetAllExpensesForGroup(groupId);
            return Ok(expenses);
        }

        [HttpGet("get-expenses-for-user")]
        public async Task<ActionResult<List<ExpenseDTO>>> GetAllExpensesForUser(int groupId, int userId)
        {
            var expenses = await _expensesService.GetAllExpensesForUser(groupId, userId);
            return Ok(expenses);
        }

        [HttpGet("get-all-expenses-grouped-by-user")]
        public async Task<ActionResult<Dictionary<string, List<ExpenseDTO>>>> GetAllExpensesGroupedByUser(int groupId)
        {
            var expensesGroupedByUser = await _expensesService.GetAllExpensesGroupedByUser(groupId);
            return Ok(expensesGroupedByUser);
        }

        [HttpPost("add-expense")]
        public async Task<ActionResult<ExpenseDTO>> AddExpense(AddExpenseDTO expense)
        {
            try
            {
                var newExpense = await _expensesService.AddExpense(expense);
                return Ok(newExpense);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to add expense");
            }
        }

        [HttpDelete("remove-expense")]
        public async Task<IActionResult> RemoveExpense(int expenseId)
        {
            var success = await _expensesService.RemoveExpense(expenseId);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategoryOfExpense(int expenseId, int newCategoryId)
        {
            var success = await _expensesService.UpdateCategoryOfExpense(expenseId, newCategoryId);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("update-description")]
        public async Task<IActionResult> UpdateDescriptionOfExpense(int expenseId, [FromBody] string newDescription)
        {
            var success = await _expensesService.UpdateDescriptionOfExpense(expenseId, newDescription);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("update-amount")]
        public async Task<IActionResult> UpdateAmountOfExpense(int expenseId, double newAmount)
        {
            var success = await _expensesService.UpdateAmountOfExpense(expenseId, newAmount);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
