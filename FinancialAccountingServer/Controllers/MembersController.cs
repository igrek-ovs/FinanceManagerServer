using FinancialAccountingServer.repositories.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAccountingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : Controller
    {
        private readonly IMemberRepository _memberRepository;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet("get-members-for-group")]
        public async Task<IActionResult> GetMembersForGroup(int groupId)
        {
            var response = await _memberRepository.GetGroupMembersAsync(groupId);
            return Ok(response);
        }

        [HttpGet("get-is-admin")]
        public async Task<IActionResult> IsMemberAdminForGroup(int groupId, int userId)
        {
            var response = await _memberRepository.IsMemberAdminForGroup(groupId, userId);
            return Ok(response);
        }

        //if the user is blocked - we unblock him, and in reverse order
        [HttpPut("block-unblock-user")]
        public async Task<IActionResult> ToggleMemberBlockStatusAsync(int groupId, int userId)
        {
            var response = _memberRepository.ToggleMemberBlockStatusAsync(groupId, userId);
            return Ok(response);
        }

    }
}
