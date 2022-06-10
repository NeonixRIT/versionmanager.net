namespace versionmanager;

/// <summary>
/// The main class to compare a local project to the latest Github release and perform actions based on the result
/// </summary>
public class VersionManager
{
    private readonly string _author;
    private readonly string _projectName;
    private readonly char _separator;
    
    private readonly Local _local;
    private Remote? _remote;

    public delegate void OutdatedEventHandler(VersionManager sender, Release e);
    public event OutdatedEventHandler? OnOutdated;
    
    public delegate void CurrentEventHandler(VersionManager sender, Release e);
    public event CurrentEventHandler? OnCurrent;
    
    public delegate void DevEventHandler(VersionManager sender, Release e);
    public event DevEventHandler? OnDev;

    /// <summary>
    /// Instantiate the VersionManager for a project
    /// </summary>
    /// <param name="author">Owner of the project's GitHub repo</param>
    /// <param name="projectName">Name of the project's GitHub repo</param>
    /// <param name="localVersion">Version as a string</param>
    /// <param name="separator">How each Version category in the version string is separated</param>
    public VersionManager(string author, string projectName, string localVersion, char separator)
    {
        _author = author;
        _projectName = projectName;
        _separator = separator;

        _local = new Local(author, projectName, localVersion, _separator);
        _remote = null;
    }
    
    /// <summary>
    /// Create a VersionManager with a default separator of '.'
    /// </summary>
    /// <param name="author">Owner of the project's GitHub repo</param>
    /// <param name="projectName">Name of the project's GitHub repo</param>
    /// <param name="localVersion">Version as a string. Each version category separated by a period</param>
    public VersionManager(string author, string projectName, string localVersion)
    {
        _author = author;
        _projectName = projectName;
        _separator = '.';

        _local = new Local(author, projectName, localVersion, _separator);
        _remote = null;
    }

    /// <summary>
    /// Compare local project version to the latest GitHub release version. Trigger events based on comparison result
    /// </summary>
    /// <returns>Result of version comparison in form of values from the Status Enum</returns>
    public Status CheckStatus()
    {
        if (_remote == null)
        {
            _remote = new Remote(_author, _projectName, _separator);
        }
        else
        {
            _remote.Refresh();
        }

        if (_local.GetVersion().CompareTo(_remote.GetVersion()) == 0)
        {
            OnCurrent?.Invoke(this, _remote.GetData());
            return Status.Current;
        }
        
        if (_local.GetVersion().CompareTo(_remote.GetVersion()) < 0)
        {
            OnOutdated?.Invoke(this, _remote.GetData());
            return Status.Outdated;
        }

        OnDev?.Invoke(this, _remote.GetData());
        return Status.Dev;
    }

    /// <summary>
    /// Get various other information about the latest GitHub release from the parsed JSON
    /// </summary>
    /// <returns>A Release class containing information related to parsed JSON from GitHub Releases API</returns>
    public Release GetData()
    {
        _remote ??= new Remote(_author, _projectName, _separator);
        return _remote.GetData();
    }
}