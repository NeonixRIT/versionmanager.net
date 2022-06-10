namespace versionmanagernet;

/// <summary>
/// Represents a parsed GitHub release API JSON. This does not contain all the information contained in the JSON
/// </summary>
public class Release : EventArgs
{
    public string? url { get; set; }
    public string? tag_name { get; set; }
    public string? name { get; set; }
    public bool? prerelease { get; set; }
    public string? published_at { get; set; }
    public string? body { get; set; }
}