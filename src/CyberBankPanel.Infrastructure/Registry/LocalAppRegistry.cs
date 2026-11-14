using System.Text.Json;

namespace CyberBankPanel.Infrastructure.Registry;

public sealed record RegisteredPanelApp(
    string AppId,
    string DisplayName,
    string Version,
    string InstallRoot,
    string ExecutablePath,
    string IconPath,
    IReadOnlyList<string> Capabilities,
    bool IsAvailable);

public sealed class LocalAppRegistry : IDisposable
{
    private readonly string registryPath;
    private readonly LocalAppManifestReader manifestReader = new();
    private readonly Dictionary<string, StoredRegistration> registrations;

    public LocalAppRegistry(string registryPath)
    {
        this.registryPath = registryPath;
        registrations = Load(registryPath);
    }

    public void Register(string installRoot)
    {
        var registration = manifestReader.Read(installRoot);
        registrations[registration.Manifest.AppId] = new StoredRegistration(
            registration.Manifest.AppId,
            registration.Manifest.DisplayName,
            registration.Manifest.Version,
            registration.InstallRoot,
            registration.ExecutablePath,
            registration.IconPath,
            registration.Manifest.Capabilities.ToArray());
        Save();
    }

    public IReadOnlyList<RegisteredPanelApp> List()
    {
        return registrations.Values
            .OrderBy(registration => registration.DisplayName, StringComparer.OrdinalIgnoreCase)
            .Select(registration => new RegisteredPanelApp(
                registration.AppId,
                registration.DisplayName,
                registration.Version,
                registration.InstallRoot,
                registration.ExecutablePath,
                registration.IconPath,
                registration.Capabilities,
                File.Exists(registration.ExecutablePath) && File.Exists(registration.IconPath)))
            .ToArray();
    }

    public void Dispose()
    {
    }

    private static Dictionary<string, StoredRegistration> Load(string registryPath)
    {
        if (!File.Exists(registryPath))
        {
            return new Dictionary<string, StoredRegistration>(StringComparer.OrdinalIgnoreCase);
        }

        var document = JsonSerializer.Deserialize<List<StoredRegistration>>(File.ReadAllText(registryPath)) ?? [];
        return document.ToDictionary(registration => registration.AppId, StringComparer.OrdinalIgnoreCase);
    }

    private void Save()
    {
        var directory = Path.GetDirectoryName(registryPath)
            ?? throw new InvalidOperationException("The registry path must have a directory.");
        Directory.CreateDirectory(directory);
        var temporaryPath = $"{registryPath}.{Guid.NewGuid():N}.tmp";
        try
        {
            File.WriteAllText(temporaryPath, JsonSerializer.Serialize(registrations.Values));
            File.Move(temporaryPath, registryPath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    private sealed record StoredRegistration(
        string AppId,
        string DisplayName,
        string Version,
        string InstallRoot,
        string ExecutablePath,
        string IconPath,
        IReadOnlyList<string> Capabilities);
}
