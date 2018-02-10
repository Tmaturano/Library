using Library.Domain.Entities;
using Library.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infra.Data.EntityConfig
{
    public class BookConfiguration : EntityTypeConfiguration<Book>
    {
        public override void Map(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Id)
                .IsRequired();

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(b => b.Description)                
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            builder.Property(b => b.AuthorId)
                .IsRequired()
                .HasColumnType("smallint");

            builder.ToTable("Books");
        }
    }
}
