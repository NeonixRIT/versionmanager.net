# versionmanager.net
A GitHub Project Version Manager that polls latest version data from GitHub repo release tag.
# Installation
### Package Managers
[nuGet](https://www.nuget.org/packages/versionmanagernet/)

Package Manager: `PM> Install-Package versionmanagernet`

.NET CLI: `> dotnet add package versionmanagernet`

### Reference DLL
[Download the latest DLL](https://www.github.com/NeonixRIT/versionmanager.net/releases/latest) and add to your solution references or dependencies directly.

# Usage
versionmanager checks the version passed to its constructor against the tag attached to the latest release on a GitHub repo. Comparing the given version string against the `tag_name` value at [https://api.github.com/repos/{author}/{projectName}/releases/latest](). This means release tag names need to be formatted specifically for this. Versionmanager doesnt support letters in version categories* and assumes a separator of a period unless told otherwise.

*A version category is a set of numbers separated by a uniform character (e.g. 2.0.3 has categories 2, 0, and 3). Using Semantic Versioning there are usually 3 version categories (major, minor, and patch) but versionmanager supports more categories as well. 
```
using versionmanagernet;

namespace Example;

class Program
{
    static void Main(string[] args)
    {
        var vm = new VersionManager("Aquatic-Labs", "Umbra-Mod-Menu", "2.0.4");

        vm.OnOutdated += (sender, eventArgs) => { Console.WriteLine("Outdated."); };
        vm.OnCurrent += (sender, eventArgs) => { Console.WriteLine("Current."); };
        vm.OnDev += (sender, eventArgs) => { Console.WriteLine("Dev."); };
        
        vm.CheckStatus();
    }
}

```