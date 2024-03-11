using FinancialAccountingServer.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAccountingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public GeneralController(IBlobService blobService) 
        {
            _blobService = blobService;
        }

        [HttpPost("upload-image")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var imgPath = await _blobService.UploadBlobAsync(file);
            return Ok(imgPath);
        }
    }
}
