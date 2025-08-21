namespace LibrariesAdministrator.Domain.Entities
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<BookByLibrary> BookByLibraries { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
