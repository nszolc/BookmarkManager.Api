using Microsoft.AspNetCore.Mvc;
using BookmarkManager.Api.Data;
using BookmarkManager.Api.DTOs.BookDto;
using BookmarkManager.Api.DTOs.TagDto;
using BookmarkManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using BookmarkManager.Api.Exceptions;


namespace BookmarkManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookmarksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookmarksController(AppDbContext context)
    {
        _context = context;
    }

    // get - wszystkie bookmarki
    [HttpGet]
    public async Task<ActionResult<List<BookmarkResponseDto>>> GetAll(
        [FromQuery] int? categoryId,
        [FromQuery] string? search,
        [FromQuery] bool? favorite)
    {
        var query = _context.Bookmarks
            .Include(b => b.Category)
            .Include(b => b.BookmarkTags)
            .ThenInclude(bt => bt.Tag)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(b => b.CategoryId == categoryId.Value);

        if (favorite.HasValue)
            query = query.Where(b => b.IsFavorite == favorite.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b =>
                b.Title.Contains(search) ||
                b.Url.Contains(search));

        var bookmarks = await query
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return Ok(bookmarks.Select(MapToDto).ToList());
    }
    [HttpPatch("{id}/favorite")]
    public async Task<ActionResult<BookmarkResponseDto>> ToggleFavorite(int id)
    {
        var bookmark = await _context.Bookmarks
                           .Include(b => b.Category)
                           .Include(b => b.BookmarkTags)
                           .ThenInclude(bt => bt.Tag)
                           .FirstOrDefaultAsync(b => b.Id == id)
                       ?? throw new NotFoundException("Bookmark", id);

        bookmark.IsFavorite = !bookmark.IsFavorite;
        await _context.SaveChangesAsync();

        return Ok(MapToDto(bookmark));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetbyId(int id)
    {
        var bookmark = await _context.Bookmarks
            .Include(b => b.Category)
            .Include(b => b.BookmarkTags)
            .ThenInclude(bt => bt.Tag)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (bookmark == null) throw new NotFoundException("Bookmark", id);

        return Ok(MapToDto(bookmark));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] BookmarkCreateDto dto)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            throw new BadRequestException($"Category with id {dto.CategoryId} does not exist");

        var bookmark = new Bookmark
        {
            Title = dto.Title,
            Url = dto.Url,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            CreatedAt = DateTime.UtcNow,
            BookmarkTags = dto.TagIds?
                .Select(tagId => new BookmarkTag { TagId = tagId })
                .ToList() ?? new List<BookmarkTag>()
        };

        _context.Bookmarks.Add(bookmark);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetbyId), new { id = bookmark.Id }, bookmark.Id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] BookmarkUpdateDto dto)
    {
        var bookmark = await _context.Bookmarks
            .Include(b => b.BookmarkTags)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bookmark == null)
        {
            throw new NotFoundException("Bookmarks", id);
        }

        bookmark.Title = dto.Title;
        bookmark.Url = dto.Url;
        bookmark.Description = dto.Description;
        bookmark.CategoryId = dto.CategoryId;

        bookmark.BookmarkTags.Clear();
        if (dto.TagIds != null)
        {
            foreach (var tagId in dto.TagIds)
            {
                bookmark.BookmarkTags.Add(new BookmarkTag { TagId = tagId });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(bookmark);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var bookmark = await _context.Bookmarks.FindAsync(id);
        if (bookmark == null) return NotFound();

        _context.Bookmarks.Remove(bookmark);
        await _context.SaveChangesAsync();
        return NoContent();

    }

    private static BookmarkResponseDto MapToDto(Bookmark b)
    {
        return new BookmarkResponseDto(
            b.Id,
            b.Title,
            b.Url,
            b.Description,
            b.IsFavorite,
            b.CreatedAt,
            b.CategoryId,
            b.Category?.Name ?? "Unknown",
            b.Category?.Color ?? "#6b7280",
            b.BookmarkTags?.Select(bt => new TagDto(bt.Tag.Id, bt.Tag.Name)).ToList()
            ?? new List<TagDto>()
        );
    }
}   