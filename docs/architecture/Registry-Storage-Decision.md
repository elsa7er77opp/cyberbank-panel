# Registry Storage Decision

The initial CyberBank Panel registry uses a **versioned, atomic local JSON file** rather than a SQLite dependency. The only metadata retained is the explicit application manifest contract: product identity, display name, version, canonical install root, canonical executable and icon paths, and user-visible capabilities. The panel does not cache passwords, source assets, databases, projects, or product-internal state.

This deliberate first-release choice avoids shipping the currently flagged native SQLite dependency observed during dependency restore. A future SQLite migration is permitted only after a supported dependency chain has no unresolved security advisory and the migration path preserves registry history through an atomic backup.
