# Impulse Dashboard

Impulse Dashboard is a Caliburn.Micro powered WPF host that loads first-party and partner applications as plugins. The solution ships opinionated shell services (documents, tool windows, ribbon, dialogs, logging, workflow, storage) plus shared libraries so new desktop apps can boot quickly and share UX primitives.

## Repository layout

- `Impulse.Dashboard.sln` – full workspace; `Impulse.Shared.sln` contains only the reusable libraries.
- `Impulse.Framework/Impulse.Dashboard` – the WPF host, Caliburn shell, and ribbon integration.
- `Impulse.Framework/*` / `Impulse.Shared.*` – shared services, UI components, storage, workflow, viewer controls, etc.
- `Logging/Impulse.Logging.*` – logging contracts, domain logic, UI, and supporting infrastructure.
- `Impulse.Tests` – xUnit + FluentAssertions test host.
- `docs/` – architecture and roadmap notes (`QoL-Improvements.md`).

## Prerequisites

| Tool | Version |
| --- | --- |
| Windows 10/11 | Build 17763+ (required for the WPF host) |
| Visual Studio 2022 | 17.10+ with ".NET desktop development" workload or the corresponding .NET CLI |
| .NET SDK | 10.0 preview (see `global.json`) |

Additional tooling:
- WiX Toolset 4.x for building the installer (`Impulse.Framework/Impulse.Installer`).
- SQL Server LocalDB if you plan to exercise the historical logging provider or repository projects.

## Getting started

1. Install the .NET SDK specified in `global.json` (preview bits from https://aka.ms/dotnet/download/daily).
2. Restore packages for the full stack:
   ```bash
   dotnet restore Impulse.Dashboard.sln
   ```
3. Build in Release (recommended before packaging):
   ```bash
   dotnet build Impulse.Dashboard.sln -c Release
   ```
4. Run the dashboard (Windows only):
   ```bash
   dotnet run --project Impulse.Framework/Impulse.Dashboard/Impulse.Dashboard.csproj
   ```
5. Launch `Impulse.Dashboard.AppBootstrapper` if you want to debug the Caliburn bootstrap sequence inside Visual Studio.

> **Tip:** The dashboard uses plugin discovery paths specified via `--application` and `--plugin` command-line switches or the registry keys documented in `Bootstrapper.cs`. If no entries exist you will be prompted to select an application.

## Logging

The `ILogService` now keeps log records in-memory and mirrors them to `%LOCALAPPDATA%/Impulse.Dashboard/Dashboard.log`. Use the Log Viewer tool window inside the dashboard for runtime inspection, or tail the log file when debugging startup issues. No SQL backing store is required anymore.

## Testing & Coverage

xUnit tests live in `Impulse.Tests` and already reference FluentAssertions + coverlet. Run the full suite plus coverage with:
```bash
dotnet test Impulse.Dashboard.sln --collect:"XPlat Code Coverage"
```
The log service tests under `Impulse.Tests/Logging` exercise log persistence, observer notifications, and record deletion.

## Contribution guidelines

- Follow the coding conventions enforced by StyleCop (`Configurations/stylecop.json`) and the shared MSBuild props located in `Directory.Build.props`.
- Prefer 4-space indentation, braces on new lines, and `var` when the type is obvious.
- UI assets belong in `Icons`, `Styles`, or `Theme`; remember to include them in the corresponding `.csproj` so WPF resource generation finds them.
- When touching multiple modules, write short imperative commits (`Improve ribbon layout spacing`) and capture validation steps in your PR description (`dotnet test`, screenshots/GIFs for UI changes).
- Capture UX or plugin ideas inside `docs/QoL-Improvements.md` or file an issue so the roadmap stays discoverable.

## Support

Ping Luke Berry (`lukeberry9919@gmail.com`) for access to partner applications, plugin onboarding help, or installer signing.
