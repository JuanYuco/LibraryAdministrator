namespace LibrariesAdministrator.Application.DTOs.Loan
{
    public class LoanFullInfoDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int LibraryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<LoanBook> Books { get; set; }
    }

    public class LoanBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
