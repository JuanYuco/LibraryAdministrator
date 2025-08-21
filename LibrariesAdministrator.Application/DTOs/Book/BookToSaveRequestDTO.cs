namespace LibrariesAdministrator.Application.DTOs.Book
{
    public class BookToSaveRequestDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Gender { get; set; }
        public List<int> LibrariesIds { get; set; } = new List<int>();
    }
}
