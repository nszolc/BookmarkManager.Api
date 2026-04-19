using BookmarkManager.Api.Models;

namespace BookmarkManager.Api.DTOs.CategoryDto
{
    public record CategoryResponseDto(
     int Id,
     string Name,
     string Color,
     int BookmarksCount

    );
   
}
// + BookmarkCount