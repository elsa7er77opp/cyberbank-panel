$ErrorActionPreference = 'Continue'

$executable = 'C:\Users\ahmed\AppData\Local\CyberBankSmoke\Web3CollectionStudioFixed\Web3CollectionStudio.Desktop.exe'
$reportPath = 'C:\Users\ahmed\Desktop\cyberbank\collection-studio-launch-probe.txt'
$startedAt = Get-Date

Get-Process -Name 'Web3CollectionStudio.Desktop' -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Process -FilePath $executable
Start-Sleep -Seconds 8

& {
    Write-Output ('Probe start: ' + $startedAt.ToString('o'))
    Write-Output '=== Remaining process ==='
    Get-Process -Name 'Web3CollectionStudio.Desktop' -ErrorAction SilentlyContinue |
        Select-Object Id, MainWindowTitle, Responding, StartTime |
        Format-List

    Write-Output '=== New application exceptions ==='
    Get-WinEvent -FilterHashtable @{ LogName = 'Application'; StartTime = $startedAt.AddSeconds(-1) } -ErrorAction SilentlyContinue |
        Where-Object { $_.Id -eq 1000 -or $_.Id -eq 1026 } |
        Select-Object TimeCreated, Id, ProviderName, Message |
        Format-List
} | Out-File -LiteralPath $reportPath -Encoding utf8

Get-Content -LiteralPath $reportPath
