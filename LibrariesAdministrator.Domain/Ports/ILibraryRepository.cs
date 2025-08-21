using LibrariesAdministrator.Domain.Entities;

namespace LibrariesAdministrator.Domain.Ports
{
    public interface ILibraryRepository
    {
        Task<List<Library>> GetAllAsync();
        Task<List<Library>> GetByIdCollection(List<int> idCollection);
        Task<Library> GetByIdAsync(int id);
        Task<Library> CreateAsync(Library library);
        Task<Library> UpdateAsync(Library library);
    }
}
