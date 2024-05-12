using FinancialAccountingServer.Data;
using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;
using FinancialAccountingServer.repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FinancialAccountingServer.repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly FinancialAppContext _context;

        public GroupRepository(FinancialAppContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<List<Group>> GetUserGroupsAsync(int userId)
        {
            return await _context.GroupMembers
                .Where(gm => gm.UserId == userId)
                .Select(gm => gm.Group)
                .ToListAsync();
        }

        public async Task<List<Group>> GetUserAdminGroupsAsync(int userId)
        {
            return await _context.GroupMembers
                .Where(gm => gm.UserId == userId && gm.IsAdmin)
                .Select(gm => gm.Group)
                .ToListAsync();
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public async Task<Group> AddGroupAsync(GroupDTO groupDTO)
        {
            var salt = GenerateSalt();
            var group = new Group
            {
                Name = groupDTO.Name,
                PasswordSalt = salt,
                PasswordHash = HashPassword(groupDTO.Password, salt)
            };

            var addedGroup = await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            _context.GroupMembers.Add(new GroupMembership { UserId = groupDTO.UserId, GroupId = addedGroup.Entity.Id, IsAdmin = true });
            await _context.SaveChangesAsync();

            return addedGroup.Entity;
        }

        public async Task<bool> RemoveGroupAsync(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
                return false;

            var groupMembers = await _context.GroupMembers.Where(gm => gm.GroupId == groupId).ToListAsync();
            _context.GroupMembers.RemoveRange(groupMembers);

            _context.Groups.Remove(group);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateGroupNameAsync(int groupId, string newName)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
                return false;

            group.Name = newName;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// This method can be used when user trying to enter (or re-enter some group)
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="password"></param>
        /// <param name="userId">User, that is trying to enter a group</param>
        /// <returns>ID of group, which entered. Or it can return -1 if entering failed</returns>
        public async Task<int> EnterGroupAsync(string groupName, string password, int userId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
            if (group == null)
                return -1;

            var groupMember = await _context.GroupMembers.Where(g => g.GroupId == group.Id && g.UserId == userId)
                .FirstOrDefaultAsync();
            if (groupMember != null)
                return -1;

            var computedHash = HashPassword(password, group.PasswordSalt);
            if (computedHash == group.PasswordHash)
            {
                var existingMembership = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId == group.Id);
                if (existingMembership != null)
                {
                    return group.Id;
                }

                _context.GroupMembers.Add(new GroupMembership { UserId = userId, GroupId = group.Id, IsAdmin = false });
                await _context.SaveChangesAsync();

                return group.Id;
            }

            return -1;
        }
    }
}
