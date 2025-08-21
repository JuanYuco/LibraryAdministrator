namespace LibrariesAdministrator.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}
