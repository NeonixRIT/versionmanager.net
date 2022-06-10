namespace versionmanagernet;

/// <summary>
/// Represents a version number to easily compare project versions
/// </summary>
internal class Version : IComparable
{
    private readonly string _version;
    private readonly string[] _versionList;
    
    /// <summary>
    /// Comparable instance of a project's version number
    /// </summary>
    /// <param name="version">a string of digits representing version categories separated by some uniform character</param>
    /// <param name="separator">The character used to separate the version categories</param>
    public Version(string version, char separator)
    {
        _version = version;
        _versionList = version.Split(separator);
    }

    /// <summary>
    /// Instantiate a default Version of 0.0.0
    /// </summary>
    public Version()
    {
        _version = "0.0.0";
        _versionList = _version.Split('.');
    }

    /// <summary>
    /// Compare each version category to tell if local version is less than, equal to, or greater than the latest GitHub release
    /// </summary>
    /// <param name="obj">latest Version local is being compared to</param>
    /// <returns>
    /// -1 if local project is Outdated
    /// 0 if versions are equal or unexpected error occurs.
    /// 1 if local project version is ahead of latest project version (Developer build)
    /// </returns>
    public int CompareTo(object? obj)
    {
        if (obj is not Version otherVersion) return 0;
        var i = 0;
        while (i < _versionList.Length && i < otherVersion._versionList.Length)
        {
            var thisVer = int.Parse(_versionList[i]);
            var otherVer = int.Parse(otherVersion._versionList[i]);
            if (thisVer < otherVer)
            {
                return -1;
            }

            if (thisVer > otherVer)
            {
                return 1;
            }
            i++;
        }
        return 0;
    }
}