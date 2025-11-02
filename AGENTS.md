# Repository Guidelines
Welcome to the Impulse Dashboard workspace—follow these practices to keep contributions predictable and review-friendly.

## Project Structure & Module Organization
- Root contains `Impulse.Dashboard.sln` for the full stack and `Impulse.Shared.sln` for shared libraries.
- The WPF host lives in `Impulse.Framework/Impulse.Dashboard` (Caliburn.Micro shell, Ribbon, Services, Theme, and Icons).
- Reusable frameworks and integrations live under `Impulse.Framework/*` and `Impulse.Shared.*`; keep feature-specific code beside its module.
- Unit tests stay in `Impulse.Tests`; shared build settings and analyzers reside in `Directory.Build.props` and `Configurations/`.
- UI assets belong in `Icons`, `Styles`, and `Theme`; include new items in the project file so WPF resource generation sees them.

## Build, Test, and Development Commands
- `dotnet restore Impulse.Dashboard.sln` ensures all projects pick up the shared target frameworks.
- `dotnet build Impulse.Dashboard.sln -c Release` compiles every desktop and shared component.
- `dotnet run --project Impulse.Framework/Impulse.Dashboard/Impulse.Dashboard.csproj` launches the dashboard for manual verification (Windows only).
- `dotnet test Impulse.Dashboard.sln --collect:"XPlat Code Coverage"` runs xUnit suites and emits Coverlet coverage data.

## Coding Style & Naming Conventions
- Use 4-space indentation, braces on new lines, and `var` for self-evident types (see `Impulse.Framework/Impulse.Dashboard/Shell/ShellViewModel.cs`).
- Follow .NET naming: PascalCase for types/methods, camelCase for locals, and `_camelCase` when private fields require emphasis.
- StyleCop analyzers configured in `Configurations/stylecop.json` run by default; fix warnings or document intentional suppressions.
- Keep XAML resource dictionaries focused per feature, and mirror asset filenames to their ribbon or tool identifiers (e.g., `Icons/Export/Home.png`).

## Testing Guidelines
- xUnit with FluentAssertions underpins unit tests; mirror the `Sort_ShouldReturnSortedList_WhenProvidedAnyList` naming pattern.
- Co-locate new tests beneath `Impulse.Tests`, matching namespace and folder names to the feature under test.
- Use `Theory` data for permutations and fall back to `Fact` for single-scenario checks.
- Capture coverage locally with `dotnet test --collect:"XPlat Code Coverage"` before large refactors.

## Commit & Pull Request Guidelines
- Write short, imperative commit subjects (e.g., `Improve ribbon layout spacing`); include context in the body when multiple modules change or behavior shifts.
- Expand in the body when touching multiple modules, calling out breaking changes or weaving impacts.
- Pull requests should link issues, list validation steps (`dotnet test`, screenshots/GIFs for UI tweaks), and highlight configuration edits (Fody, StyleCop).
- Request a reviewer familiar with the affected module and flag any migration involving `Configurations/` or new assets.

## Tooling & Configuration Tips
- Fody weavers defined in `Configurations/FodyWeavers.xml` run during build; rebuild after changing view models that rely on injection.
- Global target frameworks are set in `Directory.Build.props` (`net10.0` variants); only override when platform requirements demand it.
- When adding icons or resources, ensure they are marked as `Resource` or `Content` in the respective `.csproj` so deployment picks them up.
