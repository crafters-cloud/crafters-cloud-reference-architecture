namespace CraftersCloud.ReferenceArchitecture.AppHost;

public static class PathHelper
{
    public static string GetDirectoryPath(this AppDomain appDomain, int levelsUp)
    {
        // current directory is @"projectDir/bin/release/net9.0
        var currentDirectory = new DirectoryInfo(appDomain.BaseDirectory);
        var directory = currentDirectory.NavigateLevelsUp(levelsUp);
        return directory.FullName;
    }

    private static DirectoryInfo NavigateLevelsUp(this DirectoryInfo currentDirectory, int levels)
    {
        for (var j = 0; j < levels; j++)
        {
            currentDirectory = currentDirectory.Parent ?? throw new InvalidOperationException($"Directory {currentDirectory.FullName} does not have parent directory.");
            
        }

        return currentDirectory;
    }
}