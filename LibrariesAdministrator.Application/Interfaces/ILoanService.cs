using LibrariesAdministrator.Application.DTOs.Loan;

namespace LibrariesAdministrator.Application.Interfaces
{
    public interface ILoanService
    {
        Task<LoanCollectionResponseDTO> GetAllAsync(LoanCollectionRequestDTO request);
        Task<LoanFullInfoResponseDTO> GetByIdAsync(int id);
        Task<LoanToSaveResponseDTO> CreateAsync(LoanToSaveRequestDTO request);
        Task<LoanToSaveResponseDTO> UpdateAsync(LoanToSaveRequestDTO request);
        Task<LoanDeleteResponseDTO> DeleteAsync(int id);
    }
}
