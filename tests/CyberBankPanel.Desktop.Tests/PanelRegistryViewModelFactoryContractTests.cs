using CyberBankPanel.Desktop.Services;
using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Desktop.Tests;

public sealed class PanelRegistryViewModelFactoryContractTests : IDisposable
{
    private readonly string workspace = Path.Combine(Path.GetTempPath(), $"panel-viewmodel-{Guid.NewGuid():N}");

    [Fact]
    public void Create_loads_products_from_the_local_panel_registry_only()
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
        using (var registry = new LocalAppRegistry(registryPath))
        {
            registry.Register(installRoot);
        }

        var panel = new PanelRegistryViewModelFactory().Create(registryPath);

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
