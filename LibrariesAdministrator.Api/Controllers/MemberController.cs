using LibrariesAdministrator.Application.DTOs.Member;
using LibrariesAdministrator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesAdministrator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            this._memberService = memberService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _memberService.GetAllAsync(new Application.DTOs.Member.MemberCollectionRequestDTO());
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.EntityCollection);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {
            var response = await _memberService.GetByIdAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.Entity);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MemberToSaveRequestDTO request)
        {
            var response = await _memberService.CreateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.UserMessage);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(MemberToSaveRequestDTO request)
        {
            var response = await _memberService.UpdateAsync(request);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.UserMessage);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _memberService.DeleteAsync(id);
            if (!response.Successful)
            {
                return StatusCode(response.HttpCode, response.UserMessage);
            }

            return Ok(response.UserMessage);
        }
    }
}
