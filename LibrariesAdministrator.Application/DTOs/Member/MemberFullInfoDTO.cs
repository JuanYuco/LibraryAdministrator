namespace LibrariesAdministrator.Application.DTOs.Member
{
    public class MemberFullInfoDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}
