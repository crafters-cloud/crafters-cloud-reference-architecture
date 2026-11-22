# Completed backlog items

Newest first. Open work is in [BACKLOG.md](BACKLOG.md).

### Clean up stale branches

Completed: 2026-11-22

Archived `angular` and `caching` as tags `archive/angular` and `archive/caching` and deleted the branches (`caching` was the early caching prototype; `main` has the finished version, including cache invalidation). Deleted `complex-binding` (its PR #14 was closed) and the merged `chore/net10`.

### RA-015 Plan the next target framework

Completed: 2026-11-22 · Decision

Move to .NET 11 together with CraftersCloud.Core 4.0.0. Tracked as RA-016.

### Upgrade to .NET 10, Core 3.1 and Aspire 13

Completed: 2026-10-03 · PR #15

Targets `net10.0`, CraftersCloud.Core 3.1.0-preview.3, Aspire 13.6.0 with the `Aspire.AppHost.Sdk` project SDK, EF Core / ASP.NET Core 10.0.12, `global.json`, Dependabot. Unfinished Service Bus work was moved to branch `wip/service-bus-integration-events` (see RA-010).
