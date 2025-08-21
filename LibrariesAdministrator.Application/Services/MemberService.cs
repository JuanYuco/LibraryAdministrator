using LibrariesAdministrator.Application.DTOs.Common;
using LibrariesAdministrator.Application.DTOs.Member;
using LibrariesAdministrator.Application.Interfaces;
using LibrariesAdministrator.Domain.Ports;
using System.Net.Mail;

namespace LibrariesAdministrator.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            this._memberRepository = memberRepository;
        }

        public async Task<MemberCollectionResponseDTO> GetAllAsync(MemberCollectionRequestDTO request)
        {
            var result = new MemberCollectionResponseDTO() { Successful = false };

            try
            {
                result.EntityCollection = new List<MemberDTO>();
                var membersEntities = await _memberRepository.GetAllAsync();
                foreach (var memberEntity in membersEntities)
                {
                    result.EntityCollection.Add(new MemberDTO
                    {
                        Id = memberEntity.Id,
                        FullName = memberEntity.FullName,
                        CellPhoneNumber = memberEntity.CellPhoneNumber,
                        Email = memberEntity.Email,
                        Status = memberEntity.Active ? "Activo" : "Inactivo"
                    });
                }

                result.Successful = true;
            } catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando los miembros.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<MemberFullInfoResponseDTO> GetByIdAsync(int id)
        {
            var result = new MemberFullInfoResponseDTO() { Successful = false };

            try
            {
                var memberEntity= await _memberRepository.GetByIdAsync(id);
                if (memberEntity == null)
                {
                    result.UserMessage = "El miembro no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                var member = new MemberFullInfoDTO
                {
                    Id = memberEntity.Id,
                    FullName = memberEntity.FullName,
                    CellPhoneNumber = memberEntity.CellPhoneNumber,
                    Email = memberEntity.Email,
                    Active = memberEntity.Active
                };

                result.Entity = member;
                result.Successful = true;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error consultando los miembros.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<MemberToSaveResponseDTO> CreateAsync(MemberToSaveRequestDTO request)
        {
            var result = new MemberToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateMemberToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                await _memberRepository.CreateAsync(new Domain.Entities.Member
                {
                    FullName = request.FullName,
                    CellPhoneNumber = request.CellPhoneNumber,
                    Email = request.Email,
                    Active = request.Active,
                    IsDeleted = false
                });

                result.UserMessage = "El miembro fue creado exitosamente.";
                result.Successful = true;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error creando el miembro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        public async Task<MemberToSaveResponseDTO> UpdateAsync(MemberToSaveRequestDTO request)
        {
            var result = new MemberToSaveResponseDTO() { Successful = false };

            try
            {
                var validationResult = ValidateMemberToSave(request);
                if (!validationResult.Successful)
                {
                    result.UserMessage = validationResult.UserMessage;
                    result.HttpCode = 400;
                    return result;
                }

                var memberEntity = await _memberRepository.GetByIdAsync(request.Id);
                if (memberEntity == null)
                {
                    result.UserMessage = "No existe el miembro.";
                    result.HttpCode = 400;
                    return result;
                }

                memberEntity.FullName = request.FullName;
                memberEntity.CellPhoneNumber = request.CellPhoneNumber;
                memberEntity.Email = request.Email;
                memberEntity.Active = request.Active;
                await _memberRepository.UpdateAsync(memberEntity);

                result.UserMessage = "El miembro fue actualizado exitosamente.";
                result.Successful = true;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error actualizando el miembro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }

        private ResponseBase ValidateMemberToSave(MemberToSaveRequestDTO request)
        {
            var result = new ResponseBase() { Successful = false };

            List<string> errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                errorMessages.Add("El nombre es obligatorio");
            }

            if (request.FullName.Length > 100)
            {
                errorMessages.Add("El nombre supera los 100 caracteres");
            }

            if (string.IsNullOrEmpty(request.CellPhoneNumber))
            {
                errorMessages.Add("El número celular es obligatoria");
            }

            if (request.CellPhoneNumber.Length > 20)
            {
                errorMessages.Add("El número celular supera los 20 caracteres");
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                errorMessages.Add("El correo es obligatoria");
            }

            if (request.Email.Length > 100)
            {
                errorMessages.Add("El correo supera los 100 caracteres");
            }

            if (!IsValidEmail(request.Email))
            {
                errorMessages.Add("El correo no es válido");
            }

            if (errorMessages.Count > 0)
            {
                result.UserMessage = string.Join(",", errorMessages);
                return result;
            }

            result.Successful = true;
            return result;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<MemberDeleteResponseDTO> DeleteAsync(int id)
        {
            var result = new MemberDeleteResponseDTO() { Successful = true };

            try
            {
                var memberEntity = await _memberRepository.GetByIdAsync(id);
                if (memberEntity == null)
                {
                    result.UserMessage = "El miembro no existe.";
                    result.HttpCode = 400;
                    return result;
                }

                memberEntity.IsDeleted = true;
                if (memberEntity.Loans.Any(x => x.EndDate == null && !x.IsDeleted))
                {
                    result.UserMessage = "El miembro no se puede eliminar porque tiene por lo menos un prestamo activo.";
                    result.HttpCode = 409;
                    return result;
                }

                await _memberRepository.UpdateAsync(memberEntity);

                result.UserMessage = "El miembro fue eliminado.";
                result.Successful = true;
            }
            catch (Exception ex)
            {
                result.UserMessage = "Ocurrió un error eliminando el miembro.";
                result.InternalErrorMessage = ex.Message;
                result.HttpCode = 500;
            }

            return result;
        }
    }
}
