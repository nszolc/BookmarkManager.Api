using BookmarkManager.Api.Models;

namespace BookmarkManager.Api.DTOs.CategoryDto
{
    public record CategoryCreateDto(
    string Name,
    string Color
    );
    
}
