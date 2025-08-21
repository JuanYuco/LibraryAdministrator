using LibrariesAdministrator.Domain.Entities;
using LibrariesAdministrator.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace LibrariesAdministrator.Infrastructure.Adapters
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext appDbContext)
        {
            this._context = appDbContext;
        }
        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.Include(x => x.BookByLibraries.Where(x => x.IsDeleted == false))
                .ThenInclude(x => x.Library)
                .Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Book>> GetByIdCollectionAndLibraryAsync(List<int> idCollection, int libraryId)
        {
            return await _context.Books
                .Include(x => x.BookByLibraries)
                .Where(x => idCollection.Contains(x.Id) && x.IsDeleted == false && x.BookByLibraries.Any(x => x.LibraryId == libraryId)).ToListAsync();
        }

        public async Task<List<Book>> GetByLibraryIdAsync(int libraryId)
        {
            return await _context.Books
                .Include(x => x.BookByLibraries)
                .Where(x =>  x.IsDeleted == false && x.BookByLibraries.Any(x => x.LibraryId == libraryId)).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(x => x.BookByLibraries).ThenInclude(x => x.Library)
                .Include(x => x.BookByLoans)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            var entryBook = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return entryBook.Entity;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            var entryBook = _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return entryBook.Entity;
        }
    }
}
