using System.Text.Json;
using CyberBankPanel.Domain.Registry;

namespace CyberBankPanel.Infrastructure.Registry;

public sealed record LocalAppRegistration(CyberBankAppManifest Manifest, string InstallRoot, string ExecutablePath, string IconPath);

public sealed class LocalAppManifestReader
{
    private const string ManifestFileName = "cyberbank-app.json";

    public LocalAppRegistration Read(string installRoot)
    {
        var canonicalRoot = Path.GetFullPath(installRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var manifestPath = Path.Combine(canonicalRoot, ManifestFileName);
        var document = JsonSerializer.Deserialize<ManifestDocument>(File.ReadAllText(manifestPath), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidDataException("The CyberBank application manifest is empty.");

        var manifest = CyberBankAppManifest.Create(
            document.AppId ?? throw new InvalidDataException("The manifest appId is required."),
            document.DisplayName ?? throw new InvalidDataException("The manifest displayName is required."),
            document.Version ?? throw new InvalidDataException("The manifest version is required."),
            document.ExecutablePath ?? throw new InvalidDataException("The manifest executablePath is required."),
            document.IconPath ?? throw new InvalidDataException("The manifest iconPath is required."),
            document.Capabilities ?? [],
            document.DataRootHint,
            document.MinimumPanelVersion);

        var executablePath = ResolveContainedPath(canonicalRoot, manifest.ExecutablePath);
        var iconPath = ResolveContainedPath(canonicalRoot, manifest.IconPath);
        if (!File.Exists(executablePath) || !File.Exists(iconPath))
        {
            throw new FileNotFoundException("A registered CyberBank application must provide its declared executable and icon.");
        }

        return new LocalAppRegistration(manifest, canonicalRoot, executablePath, iconPath);
    }

    private static string ResolveContainedPath(string canonicalRoot, string relativePath)
    {
        var candidate = Path.GetFullPath(Path.Combine(canonicalRoot, relativePath));
        var rootPrefix = canonicalRoot + Path.DirectorySeparatorChar;
        if (!candidate.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("Manifest paths must resolve inside the registered install root.");
        }

        return candidate;
    }

    private sealed record ManifestDocument(
        string? AppId,
        string? DisplayName,
        string? Version,
        string? ExecutablePath,
        string? IconPath,
        IReadOnlyList<string>? Capabilities,
        string? DataRootHint,
        string? MinimumPanelVersion);
}
