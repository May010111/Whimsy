using Microsoft.EntityFrameworkCore;
using Whimsy.Data;
using Whimsy.Models;

namespace Whimsy.Services;

public class TagService
{
    private readonly AppDbContext _db;

    public TagService(AppDbContext db) => _db = db;

    public async Task<List<Tag>> GetAllAsync()
        => await _db.Tags.OrderBy(t => t.Name).ToListAsync();

    public async Task<Tag> CreateAsync(string name)
    {
        var tag = new Tag { Name = name };
        _db.Tags.Add(tag);
        await _db.SaveChangesAsync();
        return tag;
    }

    public async Task RenameAsync(Tag tag, string newName)
    {
        tag.Name = newName;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tag tag)
    {
        _db.Tags.Remove(tag);
        await _db.SaveChangesAsync();
    }
}
