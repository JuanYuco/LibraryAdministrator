using LibrariesAdministrator.Domain.Entities;
using LibrariesAdministrator.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace LibrariesAdministrator.Infrastructure.Adapters
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly AppDbContext _context;

        public LibraryRepository(AppDbContext appDbContext)
        {
            this._context = appDbContext;
        }

        public async Task<List<Library>> GetAllAsync()
        {
            return await _context.Libraries.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Library>> GetByIdCollection(List<int> idCollection)
        {
            return await _context.Libraries.Where(x => idCollection.Contains(x.Id) && !x.IsDeleted).ToListAsync();
        }

        public async Task<Library> GetByIdAsync(int id)
        {
            return await _context.Libraries.Include(x => x.BookByLibraries).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Library> CreateAsync(Library library)
        {
            var entry = await _context.Libraries.AddAsync(library);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Library> UpdateAsync(Library library)
        {
            var entry = _context.Libraries.Update(library);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
