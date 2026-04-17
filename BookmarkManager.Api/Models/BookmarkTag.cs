using Azure;

namespace BookmarkManager.Api.Models
{
    public class BookmarkTag
    {
        public int BookmarkId { get; set; }
        public Bookmark Bookmark { get; set; } = null!;
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!; 
    }
} // etykieta zakładki 
  // klasa łącząca dla relacji wiele-do-wielu między Bookmark a Tag.
  // Zawiera klucze obce do obu tabel i nawigacyjne właściwości.
