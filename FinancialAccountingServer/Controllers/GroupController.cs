using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;
using FinancialAccountingServer.repositories.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAccountingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllGroupsAsync();
            return Ok(groups);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Group>>> GetUserGroupsAsync(int userId)
        {
            var groups = await _groupRepository.GetUserGroupsAsync(userId);
            return Ok(groups);
        }

        [HttpGet("admin/{userId}")]
        public async Task<ActionResult<List<Group>>> GetUserAdminGroupsAsync(int userId)
        {
            var groups = await _groupRepository.GetUserAdminGroupsAsync(userId);
            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<Group>> AddGroupAsync(GroupDTO groupDTO)
        {
            var group = await _groupRepository.AddGroupAsync(groupDTO);
            return Ok(group);
        }

        [HttpDelete("{groupId}")]
        public async Task<ActionResult> RemoveGroupAsync(int groupId)
        {
            var success = await _groupRepository.RemoveGroupAsync(groupId);
            if (success)
                return NoContent();
            return NotFound();
        }

        [HttpPut("{groupId}")]
        public async Task<ActionResult> UpdateGroupNameAsync(int groupId, string newName)
        {
            var success = await _groupRepository.UpdateGroupNameAsync(groupId, newName);
            if (success)
                return NoContent();
            return NotFound();
        }

        [HttpPost("enter")]
        public async Task<ActionResult> EnterGroupAsync(string groupName, string password, int userId)
        {
            var success = await _groupRepository.EnterGroupAsync(groupName, password, userId);
            return Ok(success);
        }
    }
}
