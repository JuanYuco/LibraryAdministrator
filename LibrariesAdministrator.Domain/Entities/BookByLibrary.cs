namespace LibrariesAdministrator.Domain.Entities
{
    public class BookByLibrary
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int LibraryId { get; set; }
        public bool IsDeleted { get; set; }

        public Book Book { get; set; }
        public Library Library { get; set; }
    }
}
