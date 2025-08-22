using LibrariesAdministrator.Application.DTOs.Book;
using LibrariesAdministrator.Application.DTOs.Common;
using LibrariesAdministrator.Application.Interfaces;
using LibrariesAdministrator.Domain.Entities;
using LibrariesAdministrator.Domain.Ports;

namespace LibrariesAdministrator.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILibraryRepository _libraryRepository;

        public BookService(IBookRepository bookRepository, ILibraryRepository libraryRepository)
        {
            this._bookRepository = bookRepository;
            this._libraryRepository = libraryRepository;
        }

        public async Task<BookCollectionResponseDTO> GetAllAsync(BookCollectionRequestDTO request)
        {
            var result = new BookCollectionResponseDTO() { Successful = false };

            try
            {
                result.EntityCollection = new List<BookDTO>();
                var bookEntitiesCollection = await _bookRepository.GetAllAsync();
                foreach (var bookEntity in bookEntitiesCollection)
                {
                    var book = new BookDTO()
                    {
                        Id = bookEntity.Id,
                        Title = bookEntity.Title,
                        Author = bookEntity.Author,
                        Gender = bookEntity.Gender,
                        LibrariesNames = new List<string>()
                    };

                    foreach (var bookByLibrary in bookEntity.BookByLibraries)
                    {
                        book.LibrariesNames.Add(bookByLibrary.Library.Name);
                    }
                    
                    result.EntityCollection.Add(book);
                }

                result.Successful = true;
            } catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando los libros.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<BookFullInfoResponseDTO> GetByIdAsync(int id)
        {
            var result = new BookFullInfoResponseDTO() { Successful = false };

            try
            {
                var bookEntity = await _bookRepository.GetByIdAsync(id);
                if (bookEntity == null)
                {
                    result.UserMessage = "El libro no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                var book = new BookFullInfoDTO()
                {
                    Id = bookEntity.Id,
                    Title = bookEntity.Title,
                    Author = bookEntity.Author,
                    Gender = bookEntity.Gender,
                    LibrariesIds = new List<int>()
                };

                foreach (var bookLibrary in bookEntity.BookByLibraries)
                {
                    book.LibrariesIds.Add(bookLibrary.LibraryId);
                }

                result.Entity = book;
                result.Successful = true;
                return result;
            } catch(Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando el libro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<BookMinifiedCollectionDTO> GetAllByLibraryIdAsync(int libraryId)
        {
            var result = new BookMinifiedCollectionDTO() { Successful = false };

            try
            {
                result.EntityCollection = new List<BookMinifiedDTO>();
                var booksEntities = await _bookRepository.GetByLibraryIdAsync(libraryId);
                foreach (var bookEntity in booksEntities)
                {
                    result.EntityCollection.Add(new BookMinifiedDTO()
                    {
                        Id = bookEntity.Id,
                        Title = bookEntity.Title
                    });
                }

                result.Successful = true;
                return result;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando el libro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<BookToSaveResponseDTO> CreateAsync(BookToSaveRequestDTO request)
        {
            var result = new BookToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateBookToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                var librariesCollection = await _libraryRepository.GetByIdCollection(request.LibrariesIds);
                if (librariesCollection.Count != request.LibrariesIds.Count)
                {
                    result.UserMessage = "No se puede crear el libro ya que una de las librerías fue eliminada";
                    result.HttpCode = 409;
                    return result;
                }

                var bookByLibraries = new List<BookByLibrary>();
                var bookEntity = new Book()
                {
                    Author = request.Author,
                    Gender = request.Gender,
                    Title = request.Title,
                    IsDeleted = false
                };

                foreach (var libraryId in request.LibrariesIds)
                {
                    bookByLibraries.Add(new BookByLibrary
                    {
                        Book = bookEntity,
                        LibraryId = libraryId
                    });
                }

                bookEntity.BookByLibraries = bookByLibraries;

                await _bookRepository.CreateAsync(bookEntity);

                result.UserMessage = "El libro se creó correctamente.";
                result.Successful = true;
                return result;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error creando el libro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<BookToSaveResponseDTO> UpdateAsync(BookToSaveRequestDTO request)
        {
            var result = new BookToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateBookToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                var librariesCollection = await _libraryRepository.GetByIdCollection(request.LibrariesIds);
                if (librariesCollection.Count != request.LibrariesIds.Count)
                {
                    result.UserMessage = "No se puede actualizar el libro ya que una de las librerías fue eliminada";
                    result.HttpCode = 409;
                    return result;
                }

                var bookEntity = await _bookRepository.GetByIdAsync(request.Id);
                if (bookEntity == null)
                {
                    result.UserMessage = "El libro no existe";
                    result.HttpCode = 400;
                    return result;
                }

                bookEntity.Title = request.Title;
                bookEntity.Author = request.Author;
                bookEntity.Gender = request.Gender;
                foreach (var bookByLibrary in bookEntity.BookByLibraries)
                {
                    if (!request.LibrariesIds.Any(x => bookByLibrary.LibraryId == x))
                    {
                        bookByLibrary.IsDeleted = true;
                        continue;
                    }

                    bookByLibrary.IsDeleted = false;
                }

                foreach (var libaryId in request.LibrariesIds)
                {
                    if (bookEntity.BookByLibraries.Any(x => x.LibraryId == libaryId))
                    {
                        continue;
                    }

                    bookEntity.BookByLibraries.Add(new BookByLibrary
                    {
                        Book = bookEntity,
                        LibraryId = libaryId,
                        IsDeleted = false
                    });
                }


                await _bookRepository.UpdateAsync(bookEntity);

                result.UserMessage = "El libro se actualizó correctamente.";
                result.Successful = true;
                return result;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error actualizando el libro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<BookDeleteResponseDTO> DeleteAsync(int id)
        {
            var result = new BookDeleteResponseDTO() { Successful = false };

            try
            {
                var bookEntity = await _bookRepository.GetByIdAsync(id);
                if (bookEntity == null)
                {
                    result.UserMessage = "El libro no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                if (bookEntity.BookByLoans.Any())
                {
                    result.UserMessage = "El libro no se puede eliminar porque tiene prestamos asignados.";
                    result.HttpCode = 409;
                    return result;
                }

                bookEntity.IsDeleted = true;
                foreach (var bookLibrary in bookEntity.BookByLibraries)
                {
                    bookLibrary.IsDeleted = true;
                }

                await _bookRepository.UpdateAsync(bookEntity);

                result.UserMessage = "El libro fue eliminado.";
                result.Successful = true;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error eliminando el libro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        private ResponseBase ValidateBookToSave(BookToSaveRequestDTO request)
        {
            var result = new ResponseBase() { Successful = false };

            List<string> errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                errorMessages.Add(" El título es obligatorio");
            }

            if (request.Title.Length > 100)
            {
                errorMessages.Add(" El título supera los 100 caracteres");
            }

            if (string.IsNullOrEmpty(request.Author))
            {
                errorMessages.Add(" El autor es obligatorio");
            }

            if (request.Author.Length > 100)
            {
                errorMessages.Add(" El autor supera los 100 caracteres");
            }

            if (string.IsNullOrEmpty(request.Gender))
            {
                errorMessages.Add(" El género es obligatorio");
            }

            if (request.Gender.Length > 100)
            {
                errorMessages.Add(" El género supera los 100 caracteres");
            }

            if (errorMessages.Count > 0)
            {
                result.UserMessage = string.Join(",", errorMessages);
                return result;
            }

            result.Successful = true;
            return result;
        }
    }
}
