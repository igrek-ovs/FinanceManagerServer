using FinancialAccountingServer.Data;
using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;
using FinancialAccountingServer.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountingServer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly FinancialAppContext _context;

        public CategoryService(FinancialAppContext context) 
        {
            _context = context;
        }

        public async Task AddCategory(CategoryDTO categoryDTO)
        {
            var group = await _context.Groups.FindAsync(categoryDTO.GroupId);

            var category = new ExpenseCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                GroupId = categoryDTO.GroupId,
                Group = group
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryDTO>> GetCategoriesForGroup(int groupId)
        {
            var categories = await _context.Categories
                .Include(c=>c.Group)
                .Where(c=>c.GroupId == groupId)
                .Select(c => new CategoryDTO
                {
                    Id = c.GroupId,
                    Name = c.Name,
                    Description = c.Description,
                    GroupId= groupId
                }).ToListAsync();

            return categories;
        }

        public async Task<bool> RemoveCategory(int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if(category == null)
                return false;
            
            _context.Categories.Remove(category);

            return true;
        }
    }
}
