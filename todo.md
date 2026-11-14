# CyberBank Panel — Build TODO

- [x] Define and validate the explicit local `cyberbank-app.json` manifest contract.
- [x] Enforce canonical install-root containment for executable and icon paths.
- [x] Build a local registry that retains unavailable app history without reading product-private state.
- [x] Add explicit trusted launch requests using canonical executable paths only.
- [x] Add safe local launch audit events without product-private data.
- [x] Add explicit local-folder registration that accepts only a validated `cyberbank-app.json` beside an installed executable.
- [x] Wire the explicit Launch action to the registered product path, availability gate, and safe local audit flow.
- [ ] Register Secure Code Shield and Web3 Collection Studio through explicit manifests only.
- [x] Build the neutral graphite/indigo Panel Desktop shell with product rail, registry grid, inspector, health state, and status strip.
- [x] Surface retained unavailable products as a safe UNAVAILABLE state and disable launch until their local files return.
- [x] Add missing/unavailable/invalid-manifest states and safe local diagnostics.
- [x] Package and clean-install-test CyberBank Panel Windows Alpha after product manifests and installers are stable.
- [x] Add a true Windows EXE icon, explicit app manifest, self-contained x64 publish, and per-user Inno Setup installer for CyberBank Panel.
- [ ] Diagnose and repair the reported Windows Alpha launch failure, then repeat clean-install and launch verification.
