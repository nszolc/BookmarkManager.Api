namespace BookmarkManager.Api.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<BookmarkTag> BookmarkTags { get; set; } = new List<BookmarkTag>();
    }
}
