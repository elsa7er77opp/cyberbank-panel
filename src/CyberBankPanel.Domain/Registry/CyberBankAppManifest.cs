namespace CyberBankPanel.Domain.Registry;

public sealed record CyberBankAppManifest(
    string AppId,
    string DisplayName,
    string Version,
    string ExecutablePath,
    string IconPath,
    IReadOnlyList<string> Capabilities,
    string? DataRootHint,
    string? MinimumPanelVersion)
{
    public static CyberBankAppManifest Create(
        string appId,
        string displayName,
        string version,
        string executablePath,
        string iconPath,
        IReadOnlyList<string> capabilities,
        string? dataRootHint = null,
        string? minimumPanelVersion = null)
    {
        EnsureSafeRelativePath(executablePath, nameof(executablePath));
        EnsureSafeRelativePath(iconPath, nameof(iconPath));

        return new CyberBankAppManifest(
            appId,
            displayName,
            version,
            executablePath,
            iconPath,
            capabilities,
            dataRootHint,
            minimumPanelVersion);
    }

    private static void EnsureSafeRelativePath(string path, string parameterName)
    {
        if (Path.IsPathRooted(path) || path.Split(['/', '\\']).Any(segment => segment == ".."))
        {
            throw new ArgumentException("Product paths must remain inside the registered application root.", parameterName);
        }
    }
}
