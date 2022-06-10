namespace versionmanagernet;

/// <summary>
/// Representation of project version a user is currently using.
/// </summary>
internal class Local : Project
{
    public Local(string author, string name, string version, char separator) : base(author, name, new Version(version, separator)) {}
}