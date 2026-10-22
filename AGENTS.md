# AGENTS.md — Entropy App

## Project overview
Minimal Blazor Server / Razor Pages app targeting .NET 10.0. Educational app in Russian about measuring information in messages (formula: `V = N · log₂(I)`). Single project at `/home/stepa/c#/Entropy` with entry points `Program.cs` and `App.razor`. No build or test infrastructure configured yet.

## Run & build
- Build: `dotnet build ./Entropy.csproj`
- Run: `dotnet run --project ./Entropy.csproj`  
- Development launch config at `Properties/launchSettings.json`

## Static assets & hosting
- All content served from `/wwwroot`: `index.html` (fallback), `index.cshtml` (Razor pages), CSS, JS libs (jQuery, Bootstrap). No CI/deploy configs; this is a dev/study repo.

## Key files
| File | Responsibility |
|------|----------------|
| `Program.cs` | Razor Pages + Server-Side Blazor service registration |
| `App.razor` (/welcome) | Main landing page with information theory content |
| `appsettings.json/Development.json` | App configuration (simple default settings) |

## .NET build artifacts to ignore
The following build directories are generated and should be ignored in version control:
- `bin/**`
- `obj/**`

## Current state
- No packages from NuGet dependencies file; runtime uses built-in frameworks.
- No tests (`dotnet test` would fail unless added).
- No linting, CI, or task runner configured.
