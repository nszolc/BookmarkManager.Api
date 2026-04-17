using Microsoft.EntityFrameworkCore;
using BookmarkManager.Api.Models;

namespace BookmarkManager.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BookmarkTag> BookmarkTags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // skonfiguruj composite key dla BookmarkTag i relacje. Dodaj seed data — kilka kategorii i tagów, żeby baza nie była pusta.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite primary key dla tabeli łączącej
                 modelBuilder.Entity<BookmarkTag>()
                .HasKey(bt => new { bt.BookmarkId, bt.TagId });
            // BookmarkTag → Bookmark (wiele BookmarkTagów należy do jednego Bookmarka)
            modelBuilder.Entity<BookmarkTag>()
                 .HasOne<Bookmark>(d => d.Bookmark)
                 .WithMany(p => p.BookmarkTags)
                 .HasForeignKey(bt => bt.BookmarkId)
                 .OnDelete(DeleteBehavior.Cascade);

            // BookmarkTag → Tag (wiele BookmarkTagów należy do jednego Taga)
            modelBuilder.Entity<BookmarkTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BookmarkTags)
                .HasForeignKey(bt => bt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    
    }   
}