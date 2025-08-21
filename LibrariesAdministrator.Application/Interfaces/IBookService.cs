using LibrariesAdministrator.Application.DTOs.Book;

namespace LibrariesAdministrator.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookCollectionResponseDTO> GetAllAsync(BookCollectionRequestDTO request);
        Task<BookMinifiedCollectionDTO> GetAllByLibraryIdAsync(int libraryId);
        Task<BookFullInfoResponseDTO> GetByIdAsync(int id);
        Task<BookToSaveResponseDTO> CreateAsync(BookToSaveRequestDTO request);
        Task<BookToSaveResponseDTO> UpdateAsync(BookToSaveRequestDTO request);
        Task<BookDeleteResponseDTO> DeleteAsync(int id);
    }
}
