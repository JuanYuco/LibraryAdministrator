namespace LibrariesAdministrator.Domain.Entities
{
    public class BookByLoan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int LoanId { get; set; }

        public Book Book { get; set; }
        public Loan Loan { get; set; }
    }
}
