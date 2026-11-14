using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Infrastructure.Tests;

public sealed class LocalAppManifestReaderContractTests : IDisposable
{
    private readonly string installRoot = Path.Combine(Path.GetTempPath(), $"panel-product-{Guid.NewGuid():N}");

    [Fact]
    public void Read_resolves_explicit_executable_and_icon_inside_the_registered_install_root()
    {
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

        var registration = new LocalAppManifestReader().Read(installRoot);

        Assert.Equal("secure-code-shield", registration.Manifest.AppId);
        Assert.Equal(Path.Combine(installRoot, "SecureCodeShield.exe"), registration.ExecutablePath);
        Assert.Equal(Path.Combine(installRoot, "Assets", "secure-code-shield.ico"), registration.IconPath);
    }

    [Fact]
    public void Read_rejects_a_manifest_that_tries_to_escape_the_install_root()
    {
        Directory.CreateDirectory(installRoot);
        File.WriteAllText(Path.Combine(installRoot, "cyberbank-app.json"), """
            {
              "appId": "secure-code-shield",
              "displayName": "Secure Code Shield",
              "version": "1.0.0-alpha",
              "executablePath": "../untrusted.exe",
              "iconPath": "Assets/secure-code-shield.ico",
              "capabilities": []
            }
            """);

        Assert.Throws<ArgumentException>(() => new LocalAppManifestReader().Read(installRoot));
    }

    public void Dispose()
    {
        if (Directory.Exists(installRoot))
        {
            Directory.Delete(installRoot, recursive: true);
        }
    }
}
