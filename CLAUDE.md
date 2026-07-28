# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

# Windows PowerShell Environment Rules

You are running on a **Windows 11** machine inside a native **PowerShell** terminal. You must adapt all system interactions to this environment.

## File and Path Constraints
- **Never** use Unix temporary paths like `/tmp/`.
- **Always** use Windows path structures (e.g., `.\` or `C:\...`).
- Treat the current working directory as the project root.

## Command Restrictions
- **Do not** use Unix bash utilities: `cat`, `grep`, `sed`, `awk`, or `touch`.
- **Do not** use bash heredocs or output redirection strings (e.g., `cat > file << 'EOF'`).
- **Do not** attempt to invoke the macOS `open` command or Linux `xdg-open`. Use `Start-Process` or `start` if you need to execute a file.

## Execution Rules
- Use your **built-in file-writing tools** as your primary method for creating or editing files.
- If you must fall back to terminal commands, use native PowerShell cmdlets:
  - Create file: `New-Item -Path ".\filename.ext" -ItemType File`
  - Write/Overwrite file: `Set-Content -Path ".\filename.ext" -Value 'content'`
  - Append to file: `Add-Content -Path ".\filename.ext" -Value 'content'`

## Build & Test Commands

### Prerequisites
- .NET SDK 10.0.100 (see `global.json`; `rollForward: latestFeature`)
- Install MAUI workload: `dotnet workload install maui`
- Java 17+ (for Android targets)

### Build
The repo uses `.slnx` solution files (the newer XML-based format).

```bash
# Build the library solution (all src projects)
dotnet build src/CommunityToolkit.Maui.slnx

# Build a single project
dotnet build src/CommunityToolkit.Maui/CommunityToolkit.Maui.csproj

# Build the sample app
dotnet build samples/CommunityToolkit.Maui.Sample.slnx

# Release build (required for packing)
dotnet build -c Release src/CommunityToolkit.Maui.slnx
```

### Test
Tests use **xUnit v3 with Microsoft.Testing.Platform** (MTP). Run them via `dotnet run` from the test project directory, not `dotnet test`:

```bash
# Run main unit tests
cd src/CommunityToolkit.Maui.UnitTests
dotnet run -c Release

# Run analyzer unit tests
cd src/CommunityToolkit.Maui.Analyzers.UnitTests
dotnet run -c Release

# Run source generator unit tests (Windows only)
cd src/CommunityToolkit.Maui.SourceGenerators.UnitTests
dotnet run -c Release

# With code coverage
dotnet run -c Release --coverage --coverage-output-format cobertura --results-directory "./TestResults"

# Run a single test (filter by fully-qualified name)
dotnet run -c Release --filter "FullyQualifiedName~CommunityToolkit.Maui.UnitTests.Converters.BoolToObjectConverterTests"
```

### Pack (create NuGet packages)
```bash
dotnet pack -c Release src/CommunityToolkit.Maui/CommunityToolkit.Maui.csproj
```

## Architecture

### Two-Layer Library Structure

```
CommunityToolkit.Maui.Core            ← Platform abstractions
├── Handlers/        (native handler definitions)
├── Views/           (abstract views: Alert, Snackbar, DrawingView, SemanticOrderView)
├── Layouts/         (layout managers: DockLayout, UniformItemsLayout)
├── Interfaces/
├── Primitives/
├── Platform/        (StatusBar, KeyboardExtensions)
└── Services/

CommunityToolkit.Maui                 ← User-facing controls (references Core)
├── Views/           (Popup, Expander, AvatarView, DrawingView, RatingView, LazyView)
├── Behaviors/       (AnimationBehavior, EventToCommandBehavior, MaskedBehavior, etc.)
├── Converters/      (BoolToObject, ColorConverters, EnumToBool, IsEqual, TextCase, etc.)
├── Animations/
├── Layouts/         (DockLayout, UniformItemsLayout, StateContainer)
├── Alerts/
└── Extensions/

CommunityToolkit.Maui.MediaElement    ← Standalone media player (references Core)
├── Platforms/       (Android only in this repo)
├── Handlers/
└── Views/

CommunityToolkit.Maui.Camera          ← Standalone camera package
CommunityToolkit.Maui.Maps           ← Standalone maps package
```

**Core** contains platform abstractions (handlers, interfaces, base views, layout managers). All classes in Core must be `public`. **Maui** is the consumer-facing package with XAML-ready controls, behaviors, converters, and animations — it references Core and adds BindableProperties, attached properties, and MVVM support.

### Handler Pattern (Core → Platform Bridge)

Controls follow MAUI's handler architecture:
- **Core** defines `I<Control>Handler` and `*Handler.shared.cs` with the mapper and shared logic.
- Platform-specific handler files like `*.android.cs`, `*.ios.cs`, `*.windows.cs` contain native implementations.
- The main `CommunityToolkit.Maui` project registers handlers in `AppBuilderExtensions.shared.cs`.

### Platform-Specific File Naming

File naming controls cross-platform compilation. In `Directory.Build.targets`, files are included/excluded per target framework based on their suffix or directory:

| Suffix / Directory | Included on |
|---|---|
| `*.shared.cs` | All platforms |
| `*.net.cs`, `net/` | netX.0 (non-platform TFM only) |
| `*.android.cs`, `android/` | Android only |
| `*.ios.cs`, `ios/` | iOS only |
| `*.macos.cs`, `macos/` | macOS Catalyst only |
| `*.macios.cs`, `macios/` | Both iOS and macOS Catalyst |
| `*.windows.cs`, `windows/` | Windows only |
| `*.tizen.cs`, `tizen/` | Tizen only |

Platform-specific files in NuGet-packaged projects **must** follow this convention. Sample, test, analyzer, and benchmark projects use standard `*.cs` naming.

### Multi-Targeting

Core and Maui projects target: `net10.0`, `net10.0-android`, `net10.0-ios`, `net10.0-maccatalyst`, and conditionally `net10.0-windows10.0.19041.0` (only on Windows OS) and `net10.0-tizen`.

### Source Generators & Analyzers

- **CommunityToolkit.Maui.SourceGenerators** — generates BindableProperty and AttachedBindableProperty code at compile time. Shipped as an analyzer inside the `CommunityToolkit.Maui` NuGet.
- **CommunityToolkit.Maui.Analyzers** / **CodeFixes** — Roslyn analyzers for toolkit usage correctness; also packed into the main NuGet.
- **CommunityToolkit.Maui.Camera.Analyzers** — analyzers specific to the Camera package.
- **CommunityToolkit.Maui.MediaElement.Analyzers** / **SourceGenerators** — analyzers and source generators for MediaElement.
- **CommunityToolkit.Maui.SourceGenerators.Internal** — internal utilities shared between source generator projects (the `InternalsVisibleTo` pattern).

Source generators and analyzers target `netstandard2.0`.

## Code Conventions

From `.editorconfig` and `CONTRIBUTING.md`:

- **File-scoped namespaces** (enforced as error by IDE0160)
- **Tabs for C# files**, spaces for everything else (`.editorconfig`)
- **`{ }` braces required** on all `if`/`for`/`foreach`/`do`/`while` (IDE0011 = error)
- **No `private` keyword** — omit it as it's the default accessibility level
- **`camelCase` for private/static fields** — no `_` or `s_` prefix
- **`is` for null/type checking** — prefer `if (x is null)` / `if (x is Bucket b)`; avoid the `!` null-forgiving operator
- **Use collection expressions** — `[1, 2, 3]` not `new List<int> { 1, 2, 3 }`
- **`Trace.WriteLine()` for logging** — never `Debug.WriteLine()` (stripped in Release)
- **CancellationToken required** on all `Task`/`ValueTask`-returning methods (public ones get `= default`; internal ones do not)
- **Enums** — `Unknown` at index 0 for return types, `Default` at index 0 for option types, explicit values `(0,1,2,3)` unless `[Flags]`
- **No `NotImplementedException`** — it implies unfinished work; use `NotSupportedException` or implement the feature
- Warning-as-error for nullable, XML doc, MVVM Toolkit, xUnit, and trim/AOT warnings (comprehensive list in `Directory.Build.props`)

## CI

The `dotnet-build.yml` workflow:
1. **build_sample** — builds the sample app on Windows and macOS with latest .NET SDK
2. **build_library** — builds all projects, runs all test suites, generates code coverage, creates NuGet packages
3. **sign** — signs packages via Azure Key Vault (only on tag push or non-PR to main)
4. **release-main** — pushes signed packages to GitHub Packages (non-tag pushes to main)
5. **release-nuget** — pushes to NuGet.org (tagged releases only, gated by manual approval)
