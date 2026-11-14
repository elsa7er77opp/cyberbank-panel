using System.Linq;
using CyberBankPanel.Desktop.ViewModels;
using CyberBankPanel.Infrastructure.Registry;

namespace CyberBankPanel.Desktop.Services;

public sealed class PanelRegistryViewModelFactory
{
    public MainViewModel Create(string registryPath)
    {
        using var registry = new LocalAppRegistry(registryPath);
        var products = registry.List()
            .Select(registration => new PanelProductItem(
                registration.AppId,
                registration.DisplayName,
                registration.Version,
                registration.IsAvailable,
                registration.Capabilities))
            .ToArray();
        return new MainViewModel(products);
    }
}
