using LibrariesAdministrator.Domain.Entities;

namespace LibrariesAdministrator.Domain.Ports
{
    public interface ILoanRepository
    {
        Task<List<Loan>> GetAllAsync();
        Task<Loan> GetByIdAsync(int id);
        Task<Loan> CreateAsync(Loan library);
        Task<Loan> UpdateAsync(Loan library);
    }
}
