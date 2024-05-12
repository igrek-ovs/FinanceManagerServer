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
                .Where(gm => gm.GroupId == groupId && gm.isBlocked == false)
                .Select(gm => new MemberDTO
                {
                    Id = gm.UserId,
                    Username = gm.User.Username,
                    AvatarPath = gm.User.AvatarPath,
                    IsAdmin = gm.IsAdmin,
                    FirstName = gm.User.FirstName,
                    LastName = gm.User.LastName
                })
                .ToListAsync();
        }

        public async Task<MemberDTO> GetGroupMembershipAsync(int groupId, int userId)
        {
            var memberDto = await _financialAppContext.GroupMembers
                .Where(gm => gm.GroupId == groupId && gm.UserId == userId && gm.isBlocked == false)
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

        public async Task<bool> ToggleMemberBlockStatusAsync(int groupId, int userId)
        {
            var membership = await _financialAppContext.GroupMembers.FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
            if (membership != null)
            {
                membership.isBlocked = !membership.isBlocked;
                await _financialAppContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
