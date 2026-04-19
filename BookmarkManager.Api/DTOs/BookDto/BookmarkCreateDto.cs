using BookmarkManager.Api.Models;

namespace BookmarkManager.Api.DTOs.BookDto
{
    public record BookmarkCreateDto(
  
     string Title,
     string Url,
     string? Description,
     int CategoryId,
     List<int>? TagIds

    );
    
}
