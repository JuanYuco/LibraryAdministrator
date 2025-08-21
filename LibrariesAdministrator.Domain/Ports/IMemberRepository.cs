using LibrariesAdministrator.Domain.Entities;

namespace LibrariesAdministrator.Domain.Ports
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllAsync();
        Task<Member> GetByIdAsync(int id);
        Task<Member> CreateAsync(Member library);
        Task<Member> UpdateAsync(Member library);
    }
}
