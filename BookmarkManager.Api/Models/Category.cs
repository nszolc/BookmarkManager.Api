namespace BookmarkManager.Api.Models
{
    public class Category
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty; // hex string np. "#3b82f6"
        public ICollection<Bookmark> Bookmarks { get; set; } = new List <Bookmark>();

    }

  // kategoria
} // Kolekcja Bookmarks jako nawigacja one-to-many.
