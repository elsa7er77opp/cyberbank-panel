using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using CyberBankPanel.Desktop.Services;
using CyberBankPanel.Desktop.Views;

namespace CyberBankPanel.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var registryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CyberBank",
                "Panel",
                "registry.json");
            desktop.MainWindow = new MainWindow
            {
                DataContext = new PanelRegistryViewModelFactory().Create(registryPath),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
