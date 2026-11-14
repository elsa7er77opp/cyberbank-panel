# CyberBank Panel — Production Architecture

**Status:** Architecture baseline.  
**Product:** CyberBank Panel.  
**Platform:** Windows desktop, local-first.  
**Purpose:** A separate control application that provides one place to discover, launch, inspect version information for, and monitor the local availability of CyberBank products.

## Product boundary

CyberBank Panel is an **application registry and launcher**, not a replacement shell that embeds other programs. Each CyberBank product remains a separate executable with its own local data, login state, project files, process boundaries, and lifecycle. The panel can register trusted app manifests, launch an executable, show its version/build and process health, open a local project folder, and surface a user-visible diagnostic status.

It does not read or modify a product’s database, password store, source code, secrets, assets, or internal project data. It does not create background cloud synchronization, remote administration, Telegram/Discord actions, or payment features.

## Application manifest contract

Each CyberBank product can provide an explicit local `cyberbank-app.json` manifest beside its executable. The panel registers only applications whose manifest schema is valid and whose executable exists at the expected local path.

| Field | Meaning |
|---|---|
| `appId` | Stable product ID such as `secure-code-shield` |
| `displayName` | Human-readable product name |
| `version` | Semantic build version |
| `executablePath` | Relative executable path contained within the registered install root |
| `iconPath` | Relative true-alpha app icon path |
| `dataRootHint` | Optional display-only local data location hint; never opened automatically |
| `capabilities` | User-visible, non-sensitive capability labels |
| `minimumPanelVersion` | Optional compatibility guard |

The Panel canonicalizes all paths and rejects manifests whose executable or icon resolves outside the registered application root. Launch requires an explicit user action. It does not execute manifest scripts.

## Desktop experience

The Panel uses its own neutral graphite/indigo management identity. The default workspace has a product rail, an installed-products grid, a selected-product inspector, compact product health cards, and an activity/status strip. A product card shows its actual icon, current version, installation status, and launch action. A product detail view shows registry information, available local actions, release note file, and safe troubleshooting guidance.

## Security and resilience

The registry is local SQLite plus a versioned manifest cache. Panel startup detects missing executables and marks them unavailable rather than removing the user’s registry history. All launches use an absolute, canonicalized executable path; no shell-string construction is permitted. Panel logs record app ID, launch request time, outcome, and error category, never the other application’s private data.

## First delivery

The first Panel release registers Secure Code Shield and Web3 Collection Studio through explicit manifests, lists their local availability, launches them, displays their icons/versions, and shows a safe diagnostics state. It will ship after both applications have stable manifests and Windows installers.
