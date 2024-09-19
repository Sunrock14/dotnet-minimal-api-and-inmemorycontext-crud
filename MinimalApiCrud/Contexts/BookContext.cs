using Microsoft.EntityFrameworkCore;

namespace MinimalApiCrud.Contexts
{
    class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
    }

}
