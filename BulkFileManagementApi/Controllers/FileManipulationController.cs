using Microsoft.AspNetCore.Mvc;
using BulkFileManagementApi.Models;
using BulkFileManagementApi.Services;

namespace BulkFileManagementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManipulationController : ControllerBase
    {
        private readonly IFileManipulationService _fileManipulationService;

        public FileManipulationController(IFileManipulationService fileManipulationService)
        {
            _fileManipulationService = fileManipulationService;
        }

        [HttpPost]
        public async Task<IActionResult> ManipulateFiles([FromBody] FileManipulationRequest request)
        {
            //Check model state for validation errors
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //Path Checking
            if (!Directory.Exists(request.FolderPath))
            {
                ModelState.AddModelError("FolderPath", "Folder path is required.");
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "FolderPath", new[] { "Folder path is invalid." } }
                })
               );
            }

            //Check if there is file 
            if (!Directory.EnumerateFiles(request.FolderPath).Any())
            {
                ModelState.AddModelError("FolderPath", "No files found in the specified folder.");
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "FolderPath", new[] { "No files found in the specified folder." } }
                })
              );
            }

            var result = await _fileManipulationService.ManipulateFilesAsync(request);
            return Ok(result);
        }
    }
}
