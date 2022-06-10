using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace versionmanagernet;

/// <summary>
/// Handler for retrieving and managing latest GitHub Release info
/// </summary>
internal class Remote : Project
{
    private readonly string _sUrl;
    private readonly char _separator;
    private Release _data;

    /// <summary>
    /// Create a remote and fetch and parse GitHub Release API JSON
    /// </summary>
    /// <param name="author">Owner of the project's GitHub repo</param>
    /// <param name="projectName">Name of the project's GitHub repo</param>
    /// <param name="separator">Separator used in the latest release's tag</param>
    public Remote(string author, string projectName, char separator) : base(author, projectName, new Version())
    {
        _sUrl = $"https://api.github.com/repos/{author}/{projectName}/releases/latest";
        _separator = separator;
        _data = Poll().Result;
        if (_data is {tag_name: { }}) SetVersion(new Version(_data.tag_name, _separator));
    }

    /// <summary>
    /// Attempt to GET and parse GitHub release API JSON into a Release class
    /// </summary>
    /// <returns>Async Task whose result is a Release parsed from GitHub API JSON</returns>
    /// <exception cref="InvalidOperationException">Thrown when trying to parse response as a JSON</exception>
    /// <exception cref="Exception">Thrown if GET response is not a success.</exception>
    private async Task<Release> Poll()
    {
        using var handler = new HttpClientHandler();
        using var client = new HttpClient(handler);
        var req = new HttpRequestMessage(HttpMethod.Get, _sUrl);
        var productValue = new ProductInfoHeaderValue("versionmangernet", "1.0.0");
        var commentValue = new ProductInfoHeaderValue("(+https://neonix.me/versionmanager)");
        req.Headers.UserAgent.Add(productValue);
        req.Headers.UserAgent.Add(commentValue);
        var res = await client.SendAsync(req);
        if (res.IsSuccessStatusCode)
        {
            return await res.Content.ReadFromJsonAsync<Release>() ?? throw new InvalidOperationException();
        }
        throw new Exception(await res.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// Retrieve latest release again.
    /// Called whenever status is checked from a VersionManager
    /// </summary>
    public void Refresh()
    {
        var release = Poll().Result;
        if (_data is {tag_name: { }}) SetVersion(new Version(_data.tag_name, _separator));
        _data = release;
    }

    /// <summary>
    /// Allow Release information to be retrieved outside this class, directly from a VersionManager
    /// </summary>
    /// <returns>Parsed GitHub release API JSON as Release object</returns>
    public Release GetData()
    {
        return _data;
    }
}
