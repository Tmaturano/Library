using Library.Domain.Entities;
using Library.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infra.Data.EntityConfig
{
    public class AuthorConfiguration : EntityTypeConfiguration<Author>
    {
        public override void Map(EntityTypeBuilder<Author> builder)
        {
            builder.Property(a => a.Id)
                .IsRequired();

            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(a => a.DateOfBirth)
                .IsRequired()                
                .HasColumnType("datetime");

            builder.Property(a => a.DateOfDeath)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(a => a.Genre)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            builder.ToTable("Authors");            
        }
    }
}
