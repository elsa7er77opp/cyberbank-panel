using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Desktop.Services;

public sealed class PanelRegistrationService
{
    private readonly PanelRegistryViewModelFactory viewModelFactory = new();

    public ViewModels.MainViewModel Register(string registryPath, string installRoot)
    {
        using (var registry = new LocalAppRegistry(registryPath))
        {
            registry.Register(installRoot);
        }

        return viewModelFactory.Create(registryPath);
    }
}
