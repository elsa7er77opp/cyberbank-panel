using System.Diagnostics;

namespace CyberBankPanel.Infrastructure.Registry;

public sealed class TrustedAppLauncher
{
    public ProcessStartInfo BuildStartInfo(LocalAppRegistration registration)
    {
        ArgumentNullException.ThrowIfNull(registration);
        var canonicalRoot = Path.GetFullPath(registration.InstallRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var canonicalExecutable = Path.GetFullPath(registration.ExecutablePath);
        var rootPrefix = canonicalRoot + Path.DirectorySeparatorChar;
        if (!canonicalExecutable.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The registered executable is not contained by the application install root.");
        }

        if (!File.Exists(canonicalExecutable))
        {
            throw new FileNotFoundException("The registered application executable is unavailable.", canonicalExecutable);
        }

        return new ProcessStartInfo
        {
            FileName = canonicalExecutable,
            WorkingDirectory = canonicalRoot,
            UseShellExecute = false
        };
    }
}
