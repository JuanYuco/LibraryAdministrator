using LibrariesAdministrator.Application.DTOs.Member;

namespace LibrariesAdministrator.Application.Interfaces
{
    public interface IMemberService
    {
        Task<MemberCollectionResponseDTO> GetAllAsync(MemberCollectionRequestDTO request);
        Task<MemberFullInfoResponseDTO> GetByIdAsync(int id);
        Task<MemberToSaveResponseDTO> CreateAsync(MemberToSaveRequestDTO request);
        Task<MemberToSaveResponseDTO> UpdateAsync(MemberToSaveRequestDTO request);
        Task<MemberDeleteResponseDTO> DeleteAsync(int id);
    }
}
