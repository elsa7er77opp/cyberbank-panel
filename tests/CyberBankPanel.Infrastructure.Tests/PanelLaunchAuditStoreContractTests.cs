using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Infrastructure.Tests;

public sealed class PanelLaunchAuditStoreContractTests : IDisposable
{
    private readonly string auditRoot = Path.Combine(Path.GetTempPath(), $"panel-audit-{Guid.NewGuid():N}");

    [Fact]
    public void Append_records_product_identity_and_outcome_without_installation_paths()
    {
        var store = new PanelLaunchAuditStore(auditRoot);
        var privateInstallPath = Path.Combine(auditRoot, "Products", "SecureCodeShield", "SecureCodeShield.exe");

        store.Append("secure-code-shield", "launch_requested", privateInstallPath);

        var audit = File.ReadAllText(Path.Combine(auditRoot, "launch-events.jsonl"));
        Assert.Contains("secure-code-shield", audit);
        Assert.Contains("launch_requested", audit);
        Assert.DoesNotContain(privateInstallPath, audit, StringComparison.OrdinalIgnoreCase);
    }

    public void Dispose()
    {
        if (Directory.Exists(auditRoot))
        {
            Directory.Delete(auditRoot, recursive: true);
        }
    }
}
