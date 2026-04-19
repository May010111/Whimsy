namespace Whimsy.Models;

public class JournalEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? MoodId { get; set; }
    public Mood? Mood { get; set; }
    public List<EntryTag> EntryTags { get; set; } = new();

    public string TagsDisplay => EntryTags.Any()
        ? string.Join(", ", EntryTags.Select(et => et.Tag?.Name ?? ""))
        : "—";
}
