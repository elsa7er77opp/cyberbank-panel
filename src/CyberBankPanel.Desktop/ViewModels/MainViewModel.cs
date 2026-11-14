using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CyberBankPanel.Desktop.ViewModels;

public sealed record PanelProductItem(
    string AppId,
    string DisplayName,
    string Version,
    bool IsAvailable,
    IReadOnlyList<string> Capabilities)
{
    public string AvailabilityLabel => IsAvailable ? "AVAILABLE" : "UNAVAILABLE";

    public string AvailabilityDetail => IsAvailable
        ? "Registered executable and icon are available locally."
        : "Executable or icon is not currently available at the registered local path.";
}

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel(IEnumerable<PanelProductItem>? products = null)
    {
        Products = (products ?? []).ToArray();
        SelectedProduct = Products.FirstOrDefault();
    }

    [ObservableProperty]
    public partial PanelProductItem? SelectedProduct { get; set; }

    public string ProductName => "CYBERBANK PANEL";

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "LOCAL REGISTRY READY";

    public IReadOnlyList<PanelProductItem> Products { get; }

    public void SelectProduct(PanelProductItem product)
    {
        if (Products.Contains(product))
        {
            SelectedProduct = product;
        }
    }

    public void ShowSelectedProductDiagnostics()
    {
        StatusMessage = SelectedProduct is null
            ? "SAFE DIAGNOSTICS — NO PRODUCT SELECTED"
            : $"LOCAL DIAGNOSTICS — {SelectedProduct.AvailabilityDetail}";
    }
}
