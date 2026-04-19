namespace Whimsy.Models;

public class Mood
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public List<JournalEntry> Entries { get; set; } = new();
}
