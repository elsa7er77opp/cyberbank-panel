using CyberBankPanel.Desktop.Services;

namespace CyberBankPanel.Desktop.Tests;

public sealed class PanelRegistrationServiceContractTests : IDisposable
{
    private readonly string workspace = Path.Combine(Path.GetTempPath(), $"panel-register-{Guid.NewGuid():N}");

    [Fact]
    public void Register_adds_an_explicit_local_manifest_and_returns_a_refreshed_panel_view_model()
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
        var registryPath = Path.Combine(workspace, "registry.json");

        var panel = new PanelRegistrationService().Register(registryPath, installRoot);

        var registered = Assert.Single(panel.Products);
        Assert.Equal("secure-code-shield", registered.AppId);
        Assert.True(registered.IsAvailable);
    }

    public void Dispose()
    {
        if (Directory.Exists(workspace))
        {
            Directory.Delete(workspace, recursive: true);
        }
    }
}
