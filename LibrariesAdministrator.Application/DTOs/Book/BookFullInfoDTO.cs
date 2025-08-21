using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrariesAdministrator.Application.DTOs.Book
{
    public class BookFullInfoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Gender { get; set; }
        public List<BookLibrary> Libraries { get; set; } = new List<BookLibrary>();
    }

    public class BookLibrary
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
