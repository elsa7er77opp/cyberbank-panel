using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CyberBankPanel.Domain.Registry;
using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Desktop.Services;

public sealed class PanelLaunchService
{
    private readonly TrustedAppLauncher launcher = new();

    public ProcessStartInfo Prepare(string registryPath, string auditRoot, string appId)
    {
        RegisteredPanelApp app;
        using (var registry = new LocalAppRegistry(registryPath))
        {
            app = registry.List().SingleOrDefault(item => string.Equals(item.AppId, appId, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException("The selected application is not registered with CyberBank Panel.");
        }

        if (!app.IsAvailable)
        {
            throw new FileNotFoundException("The selected registered application is unavailable.", app.ExecutablePath);
        }

        var manifest = CyberBankAppManifest.Create(
            app.AppId,
            app.DisplayName,
            app.Version,
            Path.GetRelativePath(app.InstallRoot, app.ExecutablePath),
            Path.GetRelativePath(app.InstallRoot, app.IconPath),
            app.Capabilities);
        var registration = new LocalAppRegistration(manifest, app.InstallRoot, app.ExecutablePath, app.IconPath);
        var startInfo = launcher.BuildStartInfo(registration);
        new PanelLaunchAuditStore(auditRoot).Append(app.AppId, "launch_requested", startInfo.FileName);
        return startInfo;
    }
}
