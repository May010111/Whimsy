namespace Whimsy.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EntryTag> EntryTags { get; set; } = new();
}
