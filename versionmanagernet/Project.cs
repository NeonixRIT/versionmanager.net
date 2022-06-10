namespace versionmanagernet;

/// <summary>
/// Parent class for a Local Project and a Remote Project
/// </summary>
internal class Project
{
    private readonly string _author;
    private readonly string _name;
    private Version _version;
    
    /// <param name="author">Owner of the project's repo</param>
    /// <param name="name">Name of the project's repo</param>
    /// <param name="version">Version representation of the Projects Release</param>
    protected Project(string author, string name, Version version)
    {
        _author = author;
        _name = name;
        _version = version;
    }

    /// <summary>
    /// Update project's version whenever Remote is refreshed
    /// </summary>
    /// <param name="newVersion">Version parsed from updated Release info</param>
    protected void SetVersion(Version newVersion)
    {
        _version = newVersion;
    }
    
    /// <returns>Projects Version</returns>
    public Version GetVersion()
    {
        return _version;
    }
}