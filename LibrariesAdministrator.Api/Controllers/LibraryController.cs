using LibrariesAdministrator.Application.DTOs.Library;
using LibrariesAdministrator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesAdministrator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            this._libraryService = libraryService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _libraryService.GetAllAsync(new Application.DTOs.Library.LibraryRequestDTO());
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.EntityCollection);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {
            var response = await _libraryService.GetByIdAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.Entity);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(LibraryToSaveRequestDTO request)
        {
            var response = await _libraryService.CreateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(new { response.UserMessage });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(LibraryToSaveRequestDTO request)
        {
            var response = await _libraryService.UpdateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(new { response.UserMessage });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _libraryService.DeleteAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(new { response.UserMessage });
        }
    }
}
