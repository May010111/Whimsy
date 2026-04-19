using Microsoft.EntityFrameworkCore;
using Whimsy.Data;
using Whimsy.Models;

namespace Whimsy.Services;

public class EntryService
{
    private readonly AppDbContext _db;

    public EntryService(AppDbContext db) => _db = db;

    public async Task<JournalEntry> CreateAsync(string title, string content, int? moodId, List<int> tagIds)
    {
        var entry = new JournalEntry
        {
            Title = title,
            Content = content,
            MoodId = moodId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _db.Entries.Add(entry);
        await _db.SaveChangesAsync();

        foreach (var tagId in tagIds)
            _db.EntryTags.Add(new EntryTag { JournalEntryId = entry.Id, TagId = tagId });

        await _db.SaveChangesAsync();
        return await GetByIdAsync(entry.Id) ?? entry;
    }

    public async Task<List<JournalEntry>> GetAllAsync()
        => await _db.Entries
            .Include(e => e.Mood)
            .Include(e => e.EntryTags).ThenInclude(et => et.Tag)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

    public async Task<JournalEntry?> GetByIdAsync(int id)
        => await _db.Entries
            .Include(e => e.Mood)
            .Include(e => e.EntryTags).ThenInclude(et => et.Tag)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task UpdateAsync(JournalEntry entry, string title, string content, int? moodId, List<int> tagIds)
    {
        entry.Title = title;
        entry.Content = content;
        entry.MoodId = moodId;
        entry.UpdatedAt = DateTime.Now;

        _db.EntryTags.RemoveRange(_db.EntryTags.Where(et => et.JournalEntryId == entry.Id));
        await _db.SaveChangesAsync();

        foreach (var tagId in tagIds)
            _db.EntryTags.Add(new EntryTag { JournalEntryId = entry.Id, TagId = tagId });

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(JournalEntry entry)
    {
        _db.Entries.Remove(entry);
        await _db.SaveChangesAsync();
    }
}
