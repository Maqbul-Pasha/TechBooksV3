using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechBooks.Models;

namespace TechBooks.Data;

public class TechBooksContext : IdentityDbContext
{
    public TechBooksContext(DbContextOptions options) : base(options) { }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Author>()
        .HasMany(anAuthor => anAuthor.Books) //An author can have many books
        .WithMany(aBook => aBook.Authors) //A book can have many authors
        .UsingEntity<AuthorBook>();

        modelBuilder.Entity<Category>()
        .HasMany(aCategory => aCategory.Books)
        .WithOne(aBook => aBook.Category)
        .HasForeignKey(aBook => aBook.CategoryId)
        .OnDelete(DeleteBehavior.NoAction);
    }
}


