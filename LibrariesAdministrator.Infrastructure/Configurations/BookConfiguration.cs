using LibrariesAdministrator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrariesAdministrator.Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Author).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Gender).IsRequired().HasMaxLength(100);
            builder.Property(c => c.IsDeleted).IsRequired();
        }
    }
}
