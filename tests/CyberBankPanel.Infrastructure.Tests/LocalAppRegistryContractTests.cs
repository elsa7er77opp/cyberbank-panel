using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Infrastructure.Tests;

public sealed class LocalAppRegistryContractTests : IDisposable
{
    private readonly string workspace = Path.Combine(Path.GetTempPath(), $"panel-registry-{Guid.NewGuid():N}");

    [Fact]
    public void Register_persists_explicit_manifest_metadata_without_reading_product_private_state()
    {
        var installRoot = Path.Combine(workspace, "secure-code-shield");
        Directory.CreateDirectory(Path.Combine(installRoot, "Assets"));
        File.WriteAllText(Path.Combine(installRoot, "cyberbank-app.json"), """
            {
              "appId": "secure-code-shield",
              "displayName": "Secure Code Shield",
              "version": "1.0.0-alpha",
              "executablePath": "SecureCodeShield.exe",
              "iconPath": "Assets/secure-code-shield.ico",
              "capabilities": ["Local defensive scanning"]
            }
            """);
        File.WriteAllText(Path.Combine(installRoot, "SecureCodeShield.exe"), string.Empty);
        File.WriteAllText(Path.Combine(installRoot, "Assets", "secure-code-shield.ico"), string.Empty);
        var databasePath = Path.Combine(workspace, "panel-registry.db");

        using var registry = new LocalAppRegistry(databasePath);
        registry.Register(installRoot);

        var registered = Assert.Single(registry.List());
        Assert.Equal("secure-code-shield", registered.AppId);
        Assert.Equal("Secure Code Shield", registered.DisplayName);
        Assert.True(registered.IsAvailable);
    }

    [Fact]
    public void Launcher_builds_a_direct_non_shell_start_request_for_a_registered_application()
    {
        var installRoot = Path.Combine(workspace, "secure-code-shield");
        Directory.CreateDirectory(Path.Combine(installRoot, "Assets"));
        var executablePath = Path.Combine(installRoot, "SecureCodeShield.exe");
        var iconPath = Path.Combine(installRoot, "Assets", "secure-code-shield.ico");
        File.WriteAllText(executablePath, string.Empty);
        File.WriteAllText(iconPath, string.Empty);
        var registration = new LocalAppRegistration(
            CyberBankPanel.Domain.Registry.CyberBankAppManifest.Create(
                "secure-code-shield",
                "Secure Code Shield",
                "1.0.0-alpha",
                "SecureCodeShield.exe",
                "Assets/secure-code-shield.ico",
                ["Local defensive scanning"]),
            installRoot,
            executablePath,
            iconPath);

        var startInfo = new TrustedAppLauncher().BuildStartInfo(registration);

        Assert.Equal(registration.ExecutablePath, startInfo.FileName);
        Assert.Equal(registration.InstallRoot, startInfo.WorkingDirectory);
        Assert.False(startInfo.UseShellExecute);
    }

    public void Dispose()
    {
        if (Directory.Exists(workspace))
        {
            Directory.Delete(workspace, recursive: true);
        }
    }
}
