using BookmarkManager.Api.Data;
using BookmarkManager.Api.DTOs.CategoryDto;
using Microsoft.AspNetCore.Mvc;
using BookmarkManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using BookmarkManager.Api.Exceptions;

namespace BookmarkManager.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]

  public class CategoriesController : ControllerBase
  {
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
    {
      var categories = await _context.Categories
        .OrderBy(c => c.Name)
        .Select(c => new CategoryResponseDto(
          c.Id,
          c.Name,
          c.Color,
          c.Bookmarks.Count
        ))
        .ToListAsync();

      return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult> AddCategory([FromBody] CategoryCreateDto dto)
    {
      var ifExist = await _context.Categories
        .AnyAsync(c => c.Name == dto.Name);

      if (ifExist)
      {
        throw new ConflictException($"Category with name '{dto.Name}' already exists");
      }

      var category = new Category
      {
        Name = dto.Name
      };

      _context.Categories.Add(category);
      await _context.SaveChangesAsync();

      return CreatedAtAction(
        nameof(AddCategory),
        new { id = category.Id },
        new { category.Id, category.Name }
      );
    }
 
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
      var category = await _context.Categories.FindAsync(id);

      if (category == null)
      {
        throw new NotFoundException("Category", id);
      }

      _context.Categories.Remove(category);
      await _context.SaveChangesAsync();
      return NoContent();
    }
  }
}