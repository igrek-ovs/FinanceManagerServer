using FinancialAccountingServer.Data;
using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;
using FinancialAccountingServer.repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountingServer.repositories
{
    public class MemberRepository: IMemberRepository
    {
        private readonly FinancialAppContext _financialAppContext;

        public MemberRepository(FinancialAppContext financialAppContext) 
        {
            _financialAppContext = financialAppContext;
        }

        public async Task<List<GroupMembership>> GetAllMembersAsync()
        {
            return await _financialAppContext.GroupMembers.ToListAsync();
        }

        public async Task<List<MemberDTO>> GetGroupMembersAsync(int groupId)
        {
            return await _financialAppContext.GroupMembers
                .Where(gm => gm.GroupId == groupId)
                .Select(gm => new MemberDTO
                {
                    Id = gm.UserId,
                    Username = gm.User.Username,
                    AvatarPath = gm.User.AvatarPath,
                    IsAdmin = gm.IsAdmin
                })
                .ToListAsync();
        }

        public async Task<MemberDTO> GetGroupMembershipAsync(int groupId, int userId)
        {
            var memberDto = await _financialAppContext.GroupMembers
                .Where(gm => gm.GroupId == groupId && gm.UserId == userId)
                .Select(gm => new MemberDTO
                {
                    Id = gm.UserId,
                    Username = gm.User.Username,
                    AvatarPath = gm.User.AvatarPath,
                    IsAdmin = gm.IsAdmin
                })
                .FirstOrDefaultAsync();
            return memberDto;
        }

        public async Task<bool> IsMemberAdminForGroup(int groupId, int userId)
        {
            var membership = await _financialAppContext.GroupMembers.FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
            return membership != null && membership.IsAdmin;
        }

        public async Task<bool> RemoveMemberFromGroupAsync(int groupId, int userId)
        {
            var membership = await _financialAppContext.GroupMembers.FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
            if (membership != null)
            {
                _financialAppContext.GroupMembers.Remove(membership);
                await _financialAppContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
