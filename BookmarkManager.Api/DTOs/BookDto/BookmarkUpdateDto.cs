namespace BookmarkManager.Api.DTOs.BookDto
{
    public record BookmarkUpdateDto(
     string Title,
     string Url,
     string? Description,
     int CategoryId,
     List<int>? TagIds
     );
  
}
