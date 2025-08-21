using LibrariesAdministrator.Domain.Entities;
using LibrariesAdministrator.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace LibrariesAdministrator.Infrastructure.Adapters
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext appDbContext)
        {
            this._context = appDbContext;
        }
        public async Task<List<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(x => x.Library)
                .Include(x => x.Member)
                .Include(x => x.BookByLoans).ThenInclude(x => x.Book)
                .Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Loan> GetByIdAsync(int id)
        {
            return await _context.Loans
                .Include(x => x.Library)
                .Include(x => x.Member)
                .Include(x => x.BookByLoans).ThenInclude(x => x.Book)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Loan> CreateAsync(Loan loan)
        {
            var entry = await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Loan> UpdateAsync(Loan loan)
        {
            var entry = _context.Loans.Update(loan);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
