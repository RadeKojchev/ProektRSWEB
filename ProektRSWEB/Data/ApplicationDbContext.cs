using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProektRSWEB.Models;

namespace ProektRSWEB.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Author> Author { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<BookGenre> BookGenre { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Review> Review { get; set; } = default!;
        public DbSet<UserBooks> UserBooks { get; set; } = default!;
    }
}
