using LibrariesAdministrator.Application.DTOs.Book;
using LibrariesAdministrator.Application.DTOs.Common;
using LibrariesAdministrator.Application.DTOs.Loan;
using LibrariesAdministrator.Application.Interfaces;
using LibrariesAdministrator.Domain.Entities;
using LibrariesAdministrator.Domain.Ports;

namespace LibrariesAdministrator.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILibraryRepository _libraryRepository;

        public LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, IMemberRepository memberRepository, ILibraryRepository libraryRepository)
        {
            this._loanRepository = loanRepository;
            this._bookRepository = bookRepository;
            this._memberRepository = memberRepository;
            this._libraryRepository = libraryRepository;
        }

        public async Task<LoanCollectionResponseDTO> GetAllAsync(LoanCollectionRequestDTO request)
        {
            var result = new LoanCollectionResponseDTO() { Successful = false };

            try
            {
                List<LoanDTO> loans = new List<LoanDTO>();
                var loansEntities = await _loanRepository.GetAllAsync();
                foreach (var loanEntity in loansEntities)
                {
                    var loan = new LoanDTO()
                    {
                        Id = loanEntity.Id,
                        MemberFullName = loanEntity.Member.FullName,
                        LibraryName = loanEntity.Library.Name,
                        StartDate = loanEntity.StartDate,
                        EndDate = loanEntity.EndDate,
                        BookNames = loanEntity.BookByLoans.Select(x => x.Book.Title).ToList()
                    };

                    loans.Add(loan);
                }

                result.EntityCollection = loans;
                result.Successful = true;
            } catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando los prestamos.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<LoanFullInfoResponseDTO> GetByIdAsync(int id)
        {
            var result = new LoanFullInfoResponseDTO() { Successful = false };

            try
            {
                var loanEntity = await _loanRepository.GetByIdAsync(id);
                if (loanEntity == null)
                {
                    result.UserMessage = "El prestamo no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                var loan = new LoanFullInfoDTO()
                {
                    Id = loanEntity.Id,
                    MemberId = loanEntity.MemberId,
                    LibraryId = loanEntity.LibraryId,
                    StartDate = loanEntity.StartDate,
                    EndDate = loanEntity.EndDate,
                    BooksIds = new List<int>()
                };

                foreach (var bookLibrary in loanEntity.BookByLoans)
                {
                    loan.BooksIds.Add(bookLibrary.BookId);
                }

                result.Entity = loan;
                result.Successful = true;
                return result;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando el prestamo.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<LoanToSaveResponseDTO> CreateAsync(LoanToSaveRequestDTO request)
        {
            var result = new LoanToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateLoanToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                var memberEntity = await _memberRepository.GetByIdAsync(request.MemberId);
                if (memberEntity == null || (memberEntity != null && ( memberEntity.IsDeleted || !memberEntity.Active)))
                {
                    result.UserMessage = "No se puede realizar el prestamo ya que el miembro fue eliminada o desactivado.";
                    result.HttpCode = 409;
                    return result;
                }

                var libraryEntity = await _libraryRepository.GetByIdAsync(request.LibraryId);
                if (libraryEntity == null || (libraryEntity != null && libraryEntity.IsDeleted))
                {
                    result.UserMessage = "No se puede realizar el prestamo ya que la libreria fue eliminada.";
                    result.HttpCode = 409;
                    return result;
                }

                var booksCollection = await _bookRepository.GetByIdCollectionAndLibraryAsync(request.BooksIds, request.LibraryId);
                if (booksCollection.Count != request.BooksIds.Count)
                {
                    result.UserMessage = "No se puede realizar el prestamo ya que uno de los libros fue eliminado.";
                    result.HttpCode = 409;
                    return result;
                }

                var bookByLoans = new List<BookByLoan>();
                var loanEntity = new Loan()
                {
                    MemberId = request.MemberId,
                    LibraryId = request.LibraryId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsDeleted = false
                };

                foreach (var bookId in request.BooksIds)
                {
                    bookByLoans.Add(new BookByLoan
                    {
                        Loan = loanEntity,
                        BookId = bookId
                    });
                }

                loanEntity.BookByLoans = bookByLoans;

                await _loanRepository.CreateAsync(loanEntity);

                result.UserMessage = "El prestamo se creó correctamente.";
                result.Successful = true;
                return result;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error creando el prestamo.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<LoanToSaveResponseDTO> UpdateAsync(LoanToSaveRequestDTO request)
        {
            var result = new LoanToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateLoanToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                var loanEntity = await _loanRepository.GetByIdAsync(request.Id);
                if (loanEntity == null)
                {
                    result.UserMessage = "No existe el prestamo.";
                    result.HttpCode = 400;
                    return result;
                }

                var memberEntity = await _memberRepository.GetByIdAsync(request.MemberId);
                if (memberEntity == null || (memberEntity != null && (memberEntity.IsDeleted || !memberEntity.Active)))
                {
                    result.UserMessage = "No se puede realizar el prestamo ya que el miembro fue eliminada o desactivado.";
                    result.HttpCode = 409;
                    return result;
                }

                var libraryEntity = await _libraryRepository.GetByIdAsync(request.LibraryId);
                if (libraryEntity == null || (libraryEntity != null && libraryEntity.IsDeleted))
                {
                    result.UserMessage = "No se puede realizar el prestamo ya que la libreria fue eliminada.";
                    result.HttpCode = 409;
                    return result;
                }

                var booksCollection = await _bookRepository.GetByIdCollectionAndLibraryAsync(request.BooksIds, request.LibraryId);
                if (booksCollection.Count != request.BooksIds.Count)
                {
                    result.UserMessage = "No se puede realizar el prestamo ya que uno de los libros fue eliminado.";
                    result.HttpCode = 409;
                    return result;
                }

                loanEntity.MemberId = request.MemberId;
                loanEntity.LibraryId = request.LibraryId;
                loanEntity.StartDate = request.StartDate;
                loanEntity.EndDate = request.EndDate;
                loanEntity.BookByLoans.Clear();
                foreach (var bookId in request.BooksIds)
                {
                    loanEntity.BookByLoans.Add(new BookByLoan
                    {
                        Loan = loanEntity,
                        BookId = bookId,
                    });
                }

                await _loanRepository.UpdateAsync(loanEntity);

                result.UserMessage = "El prestamo se actualizó correctamente.";
                result.Successful = true;
                return result;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error actualizando el prestamo.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        private ResponseBase ValidateLoanToSave(LoanToSaveRequestDTO request)
        {
            var result = new ResponseBase() { Successful = false };

            List<string> errorMessages = new List<string>();
            if (request.StartDate == default(DateTime)) {
            
                errorMessages.Add("La fecha inicial es invalida.");
            }

            if (request.EndDate != null && request.StartDate >= request.EndDate)
            {
                errorMessages.Add("La fecha de entrega es invalida.");
            }

            if (request.BooksIds.Count == 0)
            {
                errorMessages.Add("Deben seleccionarse libros a prestar.");
            }

            if (errorMessages.Count > 0)
            {
                result.UserMessage = string.Join(",", errorMessages);
                return result;
            }

            result.Successful = true;
            return result;
        }

        public async Task<LoanDeleteResponseDTO> DeleteAsync(int id)
        {
            var result = new LoanDeleteResponseDTO() { Successful = true };

            try
            {
                var loanEntity = await _loanRepository.GetByIdAsync(id);
                if (loanEntity == null)
                {
                    result.UserMessage = "El prestamo no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                loanEntity.IsDeleted = true;
                await _loanRepository.UpdateAsync(loanEntity);

                result.UserMessage = "El prestamo fue eliminado.";
                result.Successful = true;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error eliminando el prestamo.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }
    }
}
