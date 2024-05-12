using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;

namespace FinancialAccountingServer.repositories.interfaces
{
    public interface IMemberRepository
    {
        Task<List<GroupMembership>> GetAllMembersAsync();

        Task<bool> ToggleMemberBlockStatusAsync(int groupId, int userId);

        Task<bool> IsMemberAdminForGroup(int groupId, int userId);

        Task<List<MemberDTO>> GetGroupMembersAsync(int groupId);

        Task<MemberDTO> GetGroupMembershipAsync(int groupId, int userId);
    }
}
