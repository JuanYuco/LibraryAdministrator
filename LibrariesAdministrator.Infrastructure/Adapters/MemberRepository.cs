using LibrariesAdministrator.Domain.Entities;
using LibrariesAdministrator.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace LibrariesAdministrator.Infrastructure.Adapters
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;
        public MemberRepository(AppDbContext appDbContext)
        {
            this._context = appDbContext;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Member> GetByIdAsync(int id)
        {
            return await _context.Members.Include(x => x.Loans).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Member> CreateAsync(Member member)
        {
            var entry = await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Member> UpdateAsync(Member member)
        {
            var entry = _context.Members.Update(member);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
