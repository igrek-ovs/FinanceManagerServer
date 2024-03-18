using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;

namespace FinancialAccountingServer.Services.interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategoriesForGroup(int groupId);

        Task AddCategory(CategoryDTO categoryDTO);

        Task<bool> RemoveCategory(int categoryId);
    }
}
