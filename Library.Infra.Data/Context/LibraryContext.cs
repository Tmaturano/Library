using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infra.Data.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            //Database.Migrate();
            //https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
