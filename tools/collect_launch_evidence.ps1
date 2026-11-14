$ErrorActionPreference = 'Continue'

$reportPath = 'C:\Users\ahmed\Desktop\cyberbank\launch-evidence.txt'
$roots = @(
    'C:\Users\ahmed\AppData\Local\Programs\CyberBank',
    'C:\Users\ahmed\AppData\Local\CyberBankSmoke',
    'C:\Users\ahmed\Desktop\cyberbank\web3-collection-studio\artifacts',
    'C:\Users\ahmed\Desktop\cyberbank\cyberbank-panel\artifacts'
)

& {
    Write-Output '=== CyberBank launch evidence ==='
    Write-Output ('Captured: ' + (Get-Date).ToString('o'))
    Write-Output ''
    Write-Output '=== Candidate executables ==='
    foreach ($root in $roots) {
        if (Test-Path -LiteralPath $root) {
            Get-ChildItem -LiteralPath $root -Filter '*.exe' -Recurse -ErrorAction SilentlyContinue |
                Select-Object FullName, Length, LastWriteTime
        }
    }

    Write-Output ''
    Write-Output '=== Candidate manifests ==='
    foreach ($root in $roots) {
        if (Test-Path -LiteralPath $root) {
            Get-ChildItem -LiteralPath $root -Filter 'cyberbank-app.json' -Recurse -ErrorAction SilentlyContinue |
                Select-Object FullName, Length, LastWriteTime
        }
    }

    Write-Output ''
    Write-Output '=== Recent Application errors (Event IDs 1000 and 1026) ==='
    $since = (Get-Date).AddHours(-8)
    Get-WinEvent -FilterHashtable @{ LogName = 'Application'; StartTime = $since } -ErrorAction SilentlyContinue |
        Where-Object { $_.Id -eq 1000 -or $_.Id -eq 1026 } |
        Select-Object -First 25 TimeCreated, Id, ProviderName, Message |
        Format-List
} | Out-File -LiteralPath $reportPath -Encoding utf8

Get-Content -LiteralPath $reportPath
