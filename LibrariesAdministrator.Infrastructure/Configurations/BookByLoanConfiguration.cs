using LibrariesAdministrator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrariesAdministrator.Infrastructure.Configurations
{
    public class BookByLoanConfiguration : IEntityTypeConfiguration<BookByLoan>
    {
        public void Configure(EntityTypeBuilder<BookByLoan> builder)
        {
            builder.ToTable("BookByLoan");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.BookId).IsRequired();
            builder.Property(c => c.LoanId).IsRequired();

            builder.HasOne(c => c.Book)
                .WithMany(v => v.BookByLoans)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Loan)
                .WithMany(v => v.BookByLoans)
                .HasForeignKey(x => x.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
