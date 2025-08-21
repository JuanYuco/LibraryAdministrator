namespace LibrariesAdministrator.Application.DTOs.Loan
{
    public class LoanDTO
    {
        public int Id { get; set; }
        public string MemberFullName { get; set; }
        public string LibraryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> BookNames { get; set; }
    }
}
