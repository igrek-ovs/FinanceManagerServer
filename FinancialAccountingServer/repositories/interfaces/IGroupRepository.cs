using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;

namespace FinancialAccountingServer.repositories.interfaces
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAllGroupsAsync();
        Task<List<Group>> GetUserGroupsAsync(int userId);
        Task<List<Group>> GetUserAdminGroupsAsync(int userId);
        Task<Group> AddGroupAsync(GroupDTO groupDTO);
        Task<bool> RemoveGroupAsync(int groupId);
        Task<bool> UpdateGroupNameAsync(int groupId, string newName);
        Task<bool> EnterGroupAsync(string groupName, string password, int userId);
    }
}
