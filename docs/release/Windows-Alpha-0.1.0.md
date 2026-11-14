# CyberBank Panel — Windows Alpha 0.1.0

**Release date:** 2026-08-23  
**Runtime:** Windows x64, self-contained .NET 10 publish  
**Installation model:** Per-user Inno Setup installer; no administrator privilege required.

## Distribution artifacts

| Artifact | Path | SHA-256 |
|---|---|---|
| Windows installer | `artifacts/installer/CyberBank-Panel-Alpha-Setup-win-x64.exe` | `8088c59afe5995f1402944a688c7e40d2f1810696853057d821d94c3512a6b21` |
| Portable ZIP | `artifacts/CyberBank-Panel-Alpha-win-x64.zip` | `4b220d4ee0fc5b40dce378eae80ccb8a6074a9d429d1f59118343688dc13383e` |
| Self-contained publish root | `artifacts/CyberBankPanel-win-x64/` | Not distributed separately from the ZIP. |

The publish payload includes a dedicated Panel ICO and an explicit `cyberbank-app.json`. The manifest exposes only public registration metadata: product identity, version, executable path, icon path, and advertised local capabilities.

## Verification evidence

| Check | Result |
|---|---|
| Full `CyberBankPanel.slnx` test suite | Passed after the icon/manifest configuration change. |
| Self-contained Windows x64 publish | Passed. |
| Installer compiler | Inno Setup 7.1.0 completed successfully. |
| Silent clean install | Passed into a fresh user-local smoke-test directory. |
| Installed payload validation | `CyberBankPanel.Desktop.exe` and `cyberbank-app.json` were both present. |
| One-time launch smoke test | Installed executable started successfully with the expected local registry state. |

## Included Alpha capabilities

CyberBank Panel is a **separate local desktop registry and launcher**. It accepts only explicit `cyberbank-app.json` manifests, canonicalizes executable and icon paths against the registered application root, keeps unavailable application history, shows availability safely, provides limited status diagnostics, and prepares direct non-shell launch requests only after an explicit user action. Local launch audit events store the application identity and outcome rather than private product paths or state.

## Product-boundary guarantees

Panel does not embed product windows, open product databases, read account credentials, inspect project or asset folders, or merge product-internal storage. A registered product remains a separate executable with its own local state.

## Alpha limitations

The installer and executable are **not code-signed**, so Windows may display reputation or publisher warnings. The current Alpha will register only applications whose installer/publish folder includes a valid `cyberbank-app.json` plus contained executable and icon. The next registration candidates are the stable Secure Code Shield and Web3 Collection Studio installer outputs.
