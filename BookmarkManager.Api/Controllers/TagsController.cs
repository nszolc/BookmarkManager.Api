using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookmarkManager.Api.Data;
using BookmarkManager.Api.DTOs.TagDto;
using Microsoft.AspNetCore.Mvc;
using BookmarkManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using BookmarkManager.Api.Exceptions;

namespace BookmarkManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TagsController  : ControllerBase
{
    private readonly AppDbContext _context;

    public TagsController (AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
    {
        var tags = await _context.Tags
            .OrderBy(t => t.Name)
            .Select(t => new TagDto(t.Id, t.Name))
            .ToListAsync();

        return Ok(tags);
    }

    [HttpPost]
    public async Task<ActionResult<Tag>> Post([FromBody] TagCreateDto tag)
    {
        var ifExist = await _context.Tags
            .AnyAsync(c => c.Name == tag.Name);

        if (ifExist)
            throw new ConflictException($"Tag with name '{tag.Name}' already exists");

        var newtag = new Tag
        {
            Name = tag.Name
        };
        _context.Tags.Add(newtag);
        await _context.SaveChangesAsync();
        return StatusCode(201, new TagDto(newtag.Id, newtag.Name));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null)
            throw new NotFoundException("Tag", id);

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}