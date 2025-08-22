using LibrariesAdministrator.Application.DTOs.Common;
using LibrariesAdministrator.Application.DTOs.Library;
using LibrariesAdministrator.Application.Interfaces;
using LibrariesAdministrator.Domain.Ports;

namespace LibrariesAdministrator.Application.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _libraryRepository;

        public LibraryService(ILibraryRepository libraryRepository)
        {
            this._libraryRepository = libraryRepository;
        }

        public async Task<LibraryCollectionResponseDTO> GetAllAsync(LibraryRequestDTO request)
        {
            var result = new LibraryCollectionResponseDTO() { Successful = false };

            try
            {
                result.EntityCollection = new List<LibraryDTO>();
                var libaryEntitiesCollection = await _libraryRepository.GetAllAsync();
                foreach (var libraryEntity in libaryEntitiesCollection)
                {
                    result.EntityCollection.Add(new LibraryDTO
                    {
                        Id = libraryEntity.Id,
                        Name = libraryEntity.Name,
                        Address = libraryEntity.Address
                    });
                }

                result.Successful = true;
            }
            catch (Exception ex) 
            {
                result.UserMessage = "Ocurrió un error consultando las bibliotecas.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<LibraryResponseDTO> GetByIdAsync(int id)
        {
            var result = new LibraryResponseDTO() { Successful = false };

            try
            {
                var libraryEntity = await _libraryRepository.GetByIdAsync(id);
                if (libraryEntity == null)
                {
                    result.UserMessage = "La biblioteca no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                result.Entity = new LibraryDTO
                {
                    Id = libraryEntity.Id,
                    Name = libraryEntity.Name,
                    Address = libraryEntity.Address
                };

                result.Successful = true;
                return result;
            } catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando la biblioteca.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<LibraryToSaveResponseDTO> CreateAsync(LibraryToSaveRequestDTO request)
        {
            var result = new LibraryToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateLibraryToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                await _libraryRepository.CreateAsync(new Domain.Entities.Library
                {
                    Name = request.Name,
                    Address = request.Address,
                    IsDeleted = false
                });

                result.UserMessage = "La biblioteca fue creada exitosamente.";
                result.Successful = true;
            } catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error creando la biblioteca.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<LibraryToSaveResponseDTO> UpdateAsync(LibraryToSaveRequestDTO request)
        {
            var result = new LibraryToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateLibraryToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                var libraryEntity = await _libraryRepository.GetByIdAsync(request.Id);
                if (libraryEntity == null)
                {
                    result.UserMessage = "No existe la biblioteca enviada.";
                    result.HttpCode = 400;
                    return result;
                }

                libraryEntity.Name = request.Name;
                libraryEntity.Address = request.Address;
                await _libraryRepository.UpdateAsync(libraryEntity);

                result.UserMessage = "La biblioteca fue actualizada exitosamente.";
                result.Successful = true;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error actualizando la biblioteca.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        private ResponseBase ValidateLibraryToSave(LibraryToSaveRequestDTO request)
        {
            var result = new ResponseBase() { Successful = false };

            List<string> errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                errorMessages.Add("El nombre de la biblioteca es obligatorio");
            }

            if (request.Name.Length > 100)
            {
                errorMessages.Add("El nombre de la biblioteca supera los 100 caracteres");
            }

            if (string.IsNullOrEmpty(request.Address))
            {
                errorMessages.Add("La dirección de la biblioteca es obligatoria");
            }

            if (request.Name.Length > 100)
            {
                errorMessages.Add("La dirección de la biblioteca supera los 100 caracteres");
            }

            if (errorMessages.Count > 0)
            {
                result.UserMessage = string.Join(",", errorMessages);
                return result;
            }

            result.Successful = true;
            return result;
        }

        public async Task<LibraryDeleteResponseDTO> DeleteAsync(int id)
        {
            var result = new LibraryDeleteResponseDTO() { Successful = false };

            try
            {
                var libraryEntity = await _libraryRepository.GetByIdAsync(id);
                if (libraryEntity == null)
                {
                    result.UserMessage = "La librería enviada no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                libraryEntity.IsDeleted = true;
                if (libraryEntity.BookByLibraries.Any(x => !x.IsDeleted))
                {
                    result.UserMessage = "La biblioteca no se puede eliminar porque tiene libros asignados.";
                    result.HttpCode = 409;
                    return result;
                }

                await _libraryRepository.UpdateAsync(libraryEntity);

                result.UserMessage = "La biblioteca fue eliminada.";
                result.Successful = true;
            } catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error eliminando la biblioteca.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }
    }
}
