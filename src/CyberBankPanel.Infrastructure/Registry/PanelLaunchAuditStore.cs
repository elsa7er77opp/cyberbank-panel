using System.Text.Json;

namespace CyberBankPanel.Infrastructure.Registry;

public sealed class PanelLaunchAuditStore
{
    private readonly object gate = new();
    private readonly string auditPath;

    public PanelLaunchAuditStore(string auditRoot)
    {
        auditPath = Path.Combine(auditRoot, "launch-events.jsonl");
    }

    public void Append(string appId, string eventType, string productPath)
    {
        var directory = Path.GetDirectoryName(auditPath)
            ?? throw new InvalidOperationException("The audit path must have a directory.");
        Directory.CreateDirectory(directory);
        var entry = new
        {
            timestampUtc = DateTimeOffset.UtcNow,
            appId,
            eventType
        };

        lock (gate)
        {
            File.AppendAllText(auditPath, JsonSerializer.Serialize(entry) + Environment.NewLine);
        }
    }
}
