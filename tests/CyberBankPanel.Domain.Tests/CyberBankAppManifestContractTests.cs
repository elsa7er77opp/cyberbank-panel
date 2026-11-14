using CyberBankPanel.Domain.Registry;

namespace CyberBankPanel.Domain.Tests;

public sealed class CyberBankAppManifestContractTests
{
    [Fact]
    public void Create_captures_only_explicit_non_sensitive_local_application_metadata()
    {
        var manifest = CyberBankAppManifest.Create(
            "secure-code-shield",
            "Secure Code Shield",
            "1.0.0-alpha",
            "SecureCodeShield.exe",
            "Assets/secure-code-shield.ico",
            ["Local defensive scanning", "Redacted reporting"],
            dataRootHint: "Local application data",
            minimumPanelVersion: "1.0.0");

        Assert.Equal("secure-code-shield", manifest.AppId);
        Assert.Equal("SecureCodeShield.exe", manifest.ExecutablePath);
        Assert.Equal("Assets/secure-code-shield.ico", manifest.IconPath);
        Assert.Equal(["Local defensive scanning", "Redacted reporting"], manifest.Capabilities);
        Assert.Equal("Local application data", manifest.DataRootHint);
    }

    [Theory]
    [InlineData(@"C:\OtherProduct\untrusted.exe", "Assets/secure-code-shield.ico")]
    [InlineData("SecureCodeShield.exe", @"C:\OtherProduct\untrusted.ico")]
    public void Create_rejects_absolute_product_paths(string executablePath, string iconPath)
    {
        Assert.Throws<ArgumentException>(() => CyberBankAppManifest.Create(
            "secure-code-shield",
            "Secure Code Shield",
            "1.0.0-alpha",
            executablePath,
            iconPath,
            ["Local defensive scanning"]));
    }

    [Theory]
    [InlineData("../OtherProduct/untrusted.exe", "Assets/secure-code-shield.ico")]
    [InlineData("SecureCodeShield.exe", "../OtherProduct/untrusted.ico")]
    public void Create_rejects_paths_that_escape_the_registered_root(string executablePath, string iconPath)
    {
        Assert.Throws<ArgumentException>(() => CyberBankAppManifest.Create(
            "secure-code-shield",
            "Secure Code Shield",
            "1.0.0-alpha",
            executablePath,
            iconPath,
            ["Local defensive scanning"]));
    }
}
