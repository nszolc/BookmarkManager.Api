namespace BookmarkManager.Api.Models
{
    public class Bookmark
    {
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<BookmarkTag> BookmarkTags { get; set; } = new List<BookmarkTag>();
    }
}  // zakładka 

