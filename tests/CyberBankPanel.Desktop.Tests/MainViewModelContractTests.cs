using CyberBankPanel.Desktop.ViewModels;

namespace CyberBankPanel.Desktop.Tests;

public sealed class MainViewModelContractTests
{
    [Fact]
    public void Panel_exposes_registered_products_and_selects_the_first_safe_registry_entry()
    {
        var shield = new PanelProductItem("secure-code-shield", "Secure Code Shield", "1.0.0-alpha", IsAvailable: true, ["Local defensive scanning"]);
        var collectionStudio = new PanelProductItem("web3-collection-studio", "Web3 Collection Studio", "0.1.0-alpha", IsAvailable: false, ["Local PNG composition"]);

        var panel = new MainViewModel([shield, collectionStudio]);

        Assert.Equal("CYBERBANK PANEL", panel.ProductName);
        Assert.Equal(2, panel.Products.Count);
        Assert.Same(shield, panel.SelectedProduct);
        Assert.Equal("LOCAL REGISTRY READY", panel.StatusMessage);
    }

    [Fact]
    public void Product_item_exposes_a_safe_unavailable_status_when_its_local_executable_is_missing()
    {
        var unavailable = new PanelProductItem("web3-collection-studio", "Web3 Collection Studio", "0.1.0-alpha", IsAvailable: false, ["Local PNG composition"]);

        Assert.Equal("UNAVAILABLE", unavailable.AvailabilityLabel);
        Assert.Equal("Executable or icon is not currently available at the registered local path.", unavailable.AvailabilityDetail);
    }

    [Fact]
    public void Panel_selects_an_explicit_product_for_the_inspector()
    {
        var shield = new PanelProductItem("secure-code-shield", "Secure Code Shield", "1.0.0-alpha", IsAvailable: true, ["Local defensive scanning"]);
        var collectionStudio = new PanelProductItem("web3-collection-studio", "Web3 Collection Studio", "0.1.0-alpha", IsAvailable: false, ["Local PNG composition"]);
        var panel = new MainViewModel([shield, collectionStudio]);

        panel.SelectProduct(collectionStudio);

        Assert.Same(collectionStudio, panel.SelectedProduct);
    }

    [Fact]
    public void Panel_surfaces_safe_diagnostics_without_revealing_product_private_details()
    {
        var unavailable = new PanelProductItem("web3-collection-studio", "Web3 Collection Studio", "0.1.0-alpha", IsAvailable: false, ["Local PNG composition"]);
        var panel = new MainViewModel([unavailable]);

        panel.ShowSelectedProductDiagnostics();

        Assert.Equal("LOCAL DIAGNOSTICS — Executable or icon is not currently available at the registered local path.", panel.StatusMessage);
    }
}
