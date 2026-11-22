# Backlog

Open work for `crafters-cloud-reference-architecture`. Completed items are in [BACKLOG-DONE.md](BACKLOG-DONE.md).

**Conventions**

- Items are grouped by priority — **High** (next up: security, broken builds), **Medium** (soon), **Low** (nice to have, ideas) — and ordered by importance within each group.
- Each item has an ID (`RA-nnn`, never reused), a title, the date it was added and a short description.
- When an item is done, move it to the top of `BACKLOG-DONE.md` and add `Completed: <date>` and a link to the PR.

## High

### RA-001 Merge PR #18: fix vulnerable packages and comment out MyGet

Added: 2026-11-22

`main` fails restore on vulnerabilities published since the .NET 10 upgrade (Aspire 13.6.0 critical, `System.Security.Cryptography.Pkcs` / `.Xml` 10.0.10). PR #18 fixes them and comments out the MyGet feed, which also fixes the failing Dependabot NuGet jobs. CI passes (35/35 tests).

### RA-002 Merge PR #16: protected endpoints return 403 when running locally

Added: 2026-11-22

`LogContextMiddleware` ran before authentication and cached the anonymous principal, so every permission check failed. Verified by running the AppHost.

### RA-003 Move to CraftersCloud.Core 3.1.0

Added: 2026-11-22

Depends on CORE-002 in crafters-cloud-core (stable 3.1.0 with the dependency fixes). Then check whether the transitive `System.Security.Cryptography.Pkcs` / `.Xml` pins from PR #18 are still needed.

## Medium

### RA-008 Remove the unused Microsoft.Identity.Web package

Added: 2026-11-22

`Infrastructure` references it, but no code calls it. Decided: remove it and keep `DummyUserAuthenticationHandler`; projects created from the template add their own authentication. It is also one of the packages that pulls in `System.Security.Cryptography.Pkcs`.

### RA-004 Integration test through the real authentication path

Added: 2026-11-22

Api tests replace `CurrentUserProvider` with `TestUserProvider`, which bypasses `ClaimsProvider` and the request principal; that is why the 403 bug (RA-002) wasn't caught. Add at least one test that resolves the user from the authenticated request.

### RA-005 Build a solution generated from the template in CI

Added: 2026-11-22

CI packs the template but never instantiates it. Install the package into a temporary template hive, run `dotnet new crafters-starter` and build the result. This would have caught the leftover `net9.0` in `Solution.csproj` during the .NET 10 upgrade.

### RA-006 Template cleanup leaves template-only projects in the generated solution

Added: 2026-11-22

After `dotnet new crafters-starter`, the solution still references `templates/ProjectTemplates.csproj`, `Feature.csproj` and `Solution.csproj`, so building the `.sln` fails. `cleanup-template.cmd` is supposed to remove them.

### RA-007 Run AppHost.Tests in CI

Added: 2026-11-22

`tools/pack.ps1` only runs `Category=integration|Category=unit`, so the Aspire AppHost tests never run.

### RA-016 Move to .NET 11 and CraftersCloud.Core 4.0.0

Added: 2026-11-22

Follows CORE-012: .NET 11, Core 4.0.0, EF Core 11, Aspire 17 and the other new major versions, including the template projects.

## Low

### RA-010 Redesign integration events / messaging

Added: 2026-11-22

Parked until a project needs it; then redesign rather than finish the existing attempt. Branch `wip/service-bus-integration-events` is a reference only (known gaps: handlers assign the user id to `IntegrationEvent.Id`, `IntegrationEventService` is a no-op, the event log has no registration or migration, it references `CraftersCloud.Core.IntegrationEvents` 2.1.0). `main` has the Service Bus emulator and a demo `ServiceBusWorker`.

### RA-011 Verify that ServiceBusWorker sends and receives messages

Added: 2026-11-22

The worker starts under the AppHost, but message flow through the Service Bus emulator hasn't been checked.

### RA-012 Investigate the Aspire "ContainerExec" critical log at startup

Added: 2026-11-22

Aspire 13.6 logs `crit: ... Watch task over Kubernetes ContainerExec resources terminated unexpectedly` (1-minute timeout). No visible effect; all resources keep running.

### RA-013 Update Testcontainers images

Added: 2026-11-22

Integration tests pin `redis:7.0` and `mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04` (the old Testcontainers defaults). Consider newer images; changing them recreates the reused local test containers.

### RA-014 Migrate the solution to `.slnx`

Added: 2026-11-22

crafters-cloud-core already uses `.slnx`. Check the template and `cleanup-template.cmd` still work.

### RA-009 Protect the `main` branch

Added: 2026-11-22

`main` allows direct pushes and force-pushes. Postponed while there is a single contributor; when that changes, require a PR with passing CI and block force-pushes.
