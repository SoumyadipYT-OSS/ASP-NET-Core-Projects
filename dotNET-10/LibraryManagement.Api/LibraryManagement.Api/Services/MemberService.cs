using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using LibraryManagement.Api.Services.Interfaces;

namespace LibraryManagement.Api.Services;


public class MemberService : IMemberService 
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository) 
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<MembersDto>> GetAllAsync() 
    {
        var members = await _memberRepository.GetAllAsync();
        return members.Select(m => new MembersDto {
            MemberId = m.MemberId,
            Name = m.Name,
            Email = m.Email,
            Phone = m.Phone,
            MembershipDate = m.MembershipDate
        });
    }

    public async Task<MembersDto?> GetByIdAsync(int id) 
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null) return null;

        return new MembersDto {
            MemberId = member.MemberId,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            MembershipDate = member.MembershipDate
        };
    }

    public async Task<int> RegisterAsync(CreateMemberRequest request) 
    {
        var member = new Members {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            MembershipDate = DateTime.Now
        };

        return await _memberRepository.AddAsync(member);
    }

    public async Task<int> UpdateAsync(MembersDto memberDto) 
    {
        var member = new Members {
            MemberId = memberDto.MemberId,
            Name = memberDto.Name,
            Email = memberDto.Email,
            Phone = memberDto.Phone,
            MembershipDate = memberDto.MembershipDate
        };

        return await _memberRepository.UpdateAsync(member);
    }

    public async Task<int> DeleteAsync(int id) 
    {
        return await _memberRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MemberHistoryDto>> GetBorrowHistoryAsync(int memberId) 
    {
        return await _memberRepository.GetBorrowHistoryAsync(memberId);
    }
}
