using LibrariesAdministrator.Domain.Entities;

namespace LibrariesAdministrator.Domain.Ports
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetByIdCollectionAndLibraryAsync(List<int> idCollection, int libraryId);
        Task<Book> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<List<Book>> GetByLibraryIdAsync(int libraryId);
    }
}
