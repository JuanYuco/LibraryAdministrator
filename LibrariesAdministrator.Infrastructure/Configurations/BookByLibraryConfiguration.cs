using LibrariesAdministrator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrariesAdministrator.Infrastructure.Configurations
{
    public class BookByLibraryConfiguration : IEntityTypeConfiguration<BookByLibrary>
    {
        public void Configure(EntityTypeBuilder<BookByLibrary> builder)
        {
            builder.ToTable("BookByLibrary");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.BookId).IsRequired();
            builder.Property(c => c.LibraryId).IsRequired();
            builder.Property(c => c.IsDeleted).IsRequired();

            builder.HasOne(c => c.Book)
                .WithMany(v => v.BookByLibraries)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Library)
                .WithMany(v => v.BookByLibraries)
                .HasForeignKey(x => x.LibraryId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
