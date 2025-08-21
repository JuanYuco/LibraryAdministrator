namespace LibrariesAdministrator.Application.DTOs.Loan
{
    public class LoanToSaveRequestDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int LibraryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> BooksIds { get; set; }
    }
}
