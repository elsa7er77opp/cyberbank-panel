using CyberBankPanel.Desktop.Services;
using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Desktop.Tests;

public sealed class PanelLaunchServiceContractTests : IDisposable
{
    private readonly string workspace = Path.Combine(Path.GetTempPath(), $"panel-launch-{Guid.NewGuid():N}");

    [Fact]
    public void Prepare_builds_a_direct_start_request_from_the_registered_app_id_and_records_a_safe_audit_event()
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
        var executablePath = Path.Combine(installRoot, "SecureCodeShield.exe");
        File.WriteAllText(executablePath, string.Empty);
        File.WriteAllText(Path.Combine(installRoot, "Assets", "secure-code-shield.ico"), string.Empty);
        var registryPath = Path.Combine(workspace, "registry.json");
        using (var registry = new LocalAppRegistry(registryPath))
        {
            registry.Register(installRoot);
        }

        var startInfo = new PanelLaunchService().Prepare(registryPath, workspace, "secure-code-shield");

        Assert.Equal(executablePath, startInfo.FileName);
        Assert.False(startInfo.UseShellExecute);
        var audit = File.ReadAllText(Path.Combine(workspace, "launch-events.jsonl"));
        Assert.Contains("launch_requested", audit);
        Assert.DoesNotContain(executablePath, audit, StringComparison.OrdinalIgnoreCase);
    }

    public void Dispose()
    {
        if (Directory.Exists(workspace))
        {
            Directory.Delete(workspace, recursive: true);
        }
    }
}
