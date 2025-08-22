using LibrariesAdministrator.Application.DTOs.Book;
using LibrariesAdministrator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesAdministrator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            this._bookService = bookService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bookService.GetAllAsync(new Application.DTOs.Book.BookCollectionRequestDTO());
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.EntityCollection);
        }

        [HttpGet("GetByLibraryId/{libraryId}")]
        public async Task<IActionResult> GetAllByLibraryId([FromRoute] int libraryId)
        {
            var response = await _bookService.GetAllByLibraryIdAsync(libraryId);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.EntityCollection);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _bookService.GetByIdAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.Entity);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(BookToSaveRequestDTO request)
        {
            var response = await _bookService.CreateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(new { response.UserMessage });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(BookToSaveRequestDTO request)
        {
            var response = await _bookService.UpdateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(new { response.UserMessage });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _bookService.DeleteAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(new { response.UserMessage });
        }
    }
}
