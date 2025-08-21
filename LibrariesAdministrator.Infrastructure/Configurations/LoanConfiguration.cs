using LibrariesAdministrator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrariesAdministrator.Infrastructure.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loan");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.MemberId).IsRequired();
            builder.Property(c => c.StartDate).IsRequired();
            builder.Property(c => c.EndDate);
            builder.Property(c => c.IsDeleted).IsRequired();

            builder.HasOne(c => c.Member)
                .WithMany(v => v.Loans)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Library)
                .WithMany(v => v.Loans)
                .HasForeignKey(x => x.LibraryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
