using Microsoft.EntityFrameworkCore;
using Whimsy.Models;

namespace Whimsy.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<JournalEntry> Entries { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<EntryTag> EntryTags { get; set; }
    public DbSet<Mood> Moods { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<EntryTag>()
            .HasKey(et => new { et.JournalEntryId, et.TagId });
    }
}
