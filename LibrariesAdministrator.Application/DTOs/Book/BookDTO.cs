namespace LibrariesAdministrator.Application.DTOs.Book
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Gender { get; set; }
        public List<string> LibrariesNames { get; set; }
    }
}
