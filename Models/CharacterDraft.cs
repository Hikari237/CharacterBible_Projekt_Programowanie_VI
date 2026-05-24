namespace CharacterBible.Models;

public class CharacterDraft
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string Gender { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Vibe { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty; 
}