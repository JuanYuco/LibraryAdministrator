namespace LibrariesAdministrator.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Gender { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<BookByLibrary> BookByLibraries { get; set; }
        public ICollection<BookByLoan> BookByLoans { get; set; }
    }
}
