namespace LibrariesAdministrator.Domain.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int LibraryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }

        public Member Member { get; set; }
        public Library Library { get; set; }
        public ICollection<BookByLoan> BookByLoans { get; set; }
    }
}
