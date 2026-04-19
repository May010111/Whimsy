using Microsoft.EntityFrameworkCore;
using Whimsy.Data;
using Whimsy.Models;

namespace Whimsy.Services;

public class SearchService
{
    private readonly AppDbContext _db;

    public SearchService(AppDbContext db) => _db = db;

    public async Task<List<JournalEntry>> SearchAsync(
        string? keyword = null,
        DateTime? from = null,
        DateTime? to = null,
        int? tagId = null,
        int? moodId = null)
    {
        var query = _db.Entries
            .Include(e => e.Mood)
            .Include(e => e.EntryTags).ThenInclude(et => et.Tag)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(e =>
                EF.Functions.Like(e.Title, $"%{keyword}%") ||
                EF.Functions.Like(e.Content, $"%{keyword}%"));

        if (from.HasValue)
            query = query.Where(e => e.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(e => e.CreatedAt <= to.Value.Date.AddDays(1));

        if (tagId.HasValue)
            query = query.Where(e => e.EntryTags.Any(et => et.TagId == tagId));

        if (moodId.HasValue)
            query = query.Where(e => e.MoodId == moodId);

        return await query.OrderByDescending(e => e.CreatedAt).ToListAsync();
    }
}
