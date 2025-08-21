using LibrariesAdministrator.Application.DTOs.Loan;
using LibrariesAdministrator.Application.Interfaces;
using LibrariesAdministrator.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesAdministrator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            this._loanService = loanService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _loanService.GetAllAsync(new Application.DTOs.Loan.LoanCollectionRequestDTO());
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.EntityCollection);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {
            var response = await _loanService.GetByIdAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.Entity);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(LoanToSaveRequestDTO request)
        {
            var response = await _loanService.CreateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.UserMessage);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(LoanToSaveRequestDTO request)
        {
            var response = await _loanService.UpdateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.UserMessage);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _loanService.DeleteAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.UserMessage);
        }
    }
}
