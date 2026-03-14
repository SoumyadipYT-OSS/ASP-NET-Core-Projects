using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase 
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService) 
    {
        _memberService = memberService;
    }

    // GET: api/members
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MembersDto>>> GetAll() 
    {
        var members = await _memberService.GetAllAsync();
        return Ok(members);
    }

    // GET: api/members/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MembersDto>> GetById(int id) 
    {
        var member = await _memberService.GetByIdAsync(id);
        if (member == null)
            return NotFound(new { Message = "Member not found" });

        return Ok(member);
    }

    // POST: api/members
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] CreateMemberRequest request) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _memberService.RegisterAsync(request);
        if (result > 0)
            return CreatedAtAction(nameof(GetById), new { id = result }, request);

        return BadRequest(new { Message = "Failed to register member" });
    }

    // PUT: api/members/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] MembersDto memberDto) 
    {
        if (id != memberDto.MemberId)
            return BadRequest(new { Message = "Member ID mismatch" });

        var result = await _memberService.UpdateAsync(memberDto);
        if (result > 0)
            return NoContent();

        return NotFound(new { Message = "Member not found" });
    }

    // DELETE: api/members/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) 
    {
        var result = await _memberService.DeleteAsync(id);
        if (result > 0)
            return NoContent();

        return NotFound(new { Message = "Member not found" });
    }

    // GET: api/members/{id}/history
    [HttpGet("{id}/history")]
    public async Task<ActionResult<IEnumerable<MemberHistoryDto>>> GetBorrowHistory(int id) 
    {
        var history = await _memberService.GetBorrowHistoryAsync(id);
        if (!history.Any())
            return NotFound(new { Message = "No borrow history found for this member" });

        return Ok(history);
    }
}
