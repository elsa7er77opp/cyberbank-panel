# CyberBank Panel

A Windows launcher reads local product manifests, registers suite applications and dispatches a selected installed executable. The domain defines the product manifest. Infrastructure reads local manifests. The desktop layer coordinates registration and launch.

## Engineering scope

The Panel is the suite host. Web3 Collection Studio's release record separately documents a host launch for that product. Other application combinations need their own installation evidence.

## Getting started

Use the .NET 10 SDK on Windows. From the repository root, build with `dotnet build CyberBankPanel.slnx`. To run the isolated domain suite, use `dotnet test tests/CyberBankPanel.Domain.Tests/CyberBankPanel.Domain.Tests.csproj`. Start the desktop application with `dotnet run --project src/CyberBankPanel.Desktop/CyberBankPanel.Desktop.csproj`.

## Status and verification

Windows Alpha 0.1.0 documented.

6 domain tests passed locally. Installed product discovery and process launch were not exercised.

## Source and evidence

- [CyberBankAppManifest.cs](src/CyberBankPanel.Domain/Registry/CyberBankAppManifest.cs)
- [LocalAppManifestReader.cs](src/CyberBankPanel.Infrastructure/Registry/LocalAppManifestReader.cs)
- [PanelLaunchService.cs](src/CyberBankPanel.Desktop/Services/PanelLaunchService.cs)
- [Windows-Alpha-0.1.0.md](docs/release/Windows-Alpha-0.1.0.md)

## Author

Ahmed Mohamed Elsayed Elreefy. Engineering work includes requirements, technical decisions, AI-assisted implementation, debugging and acceptance review.
