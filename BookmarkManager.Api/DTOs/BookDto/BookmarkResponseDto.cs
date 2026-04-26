using TagDto = BookmarkManager.Api.DTOs.TagDto;
using System;
using System.Collections.Generic;

namespace BookmarkManager.Api.DTOs.BookDto
{
    public record BookmarkResponseDto(
    int? Id,
    string Title,
    string Url,
   string? Description,
    bool IsFavorite,
    DateTime CreatedAt,
    int? CategoryId,
    string CategoryName,
    string CategoryColor,
    List<TagDto.TagDto> Tags
    );
   
    // to co zwraca klientowi
}
