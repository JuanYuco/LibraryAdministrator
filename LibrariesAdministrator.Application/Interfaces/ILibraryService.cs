using LibrariesAdministrator.Application.DTOs.Library;

namespace LibrariesAdministrator.Application.Interfaces
{
    public interface ILibraryService
    {
        Task<LibraryCollectionResponseDTO> GetAllAsync(LibraryRequestDTO request);
        Task<LibraryResponseDTO> GetByIdAsync(int id);
        Task<LibraryToSaveResponseDTO> CreateAsync(LibraryToSaveRequestDTO request);
        Task<LibraryToSaveResponseDTO> UpdateAsync(LibraryToSaveRequestDTO request);
        Task<LibraryDeleteResponseDTO> DeleteAsync(int id);
    }
}
