# Cynosura.Studio
![dev-build-status](https://dev.azure.com/CynosuraPlatform/Cynosura.Studio/_apis/build/status/Cynosura.Studio?branchName=development)

## Overview

Cynosura is a code generator that allows to quickly create projects with .NET Core and Angular.

Main features:
1. Backend is written in .NET 8
2. For API there are two options: JSON API or gRPC
3. For data storage Entity Framework Core is used
4. Business logic is implemented with [MediatR](https://github.com/jbogard/MediatR)
5. [FluentValudation](https://fluentvalidation.net/) is used for data validation
6. [AutoMapper](https://automapper.org) is used for data mapping
7. ASP.NET Core Identity and IdentityServer4 is used for user management and authentication 
8. Frontend is written in Angular 13
9. [Angular Material](https://material.angular.io) UI component library is used
10. Cynosura.Studio application is generated in Cynosura.Studio

## Getting Started

1. Start backend Cynosura.Studio.Web (`dotnet run`)
2. Start frontend Cynosura.Studio.Web.Ng (`npm run start`)
3. Open Cynosura.Studio at http://localhost:4300/
4. Create new solution in Solutions section and your new project is ready
5. Add your entity in Entities section

### Prerequisites

* .NET SDK 8
* Git (used for merging files when updating metadata or upgrading templates)

### Metadata

Cynosura.Studio allows you to create entities and enums in web UI. Entities and enums created in UI will be generated into code in your project: models, CQRS classes, EF configurations, API models and controllers, Angular models, services, and components, etc. You can change generated code as you like. When later you wish to update entity or enum metadata, those changes will be merged into your code without overwriting your custom changes.

### Solution upgrade

Upgrade feature lets you upgrade your solution to the latest template version. When you upgrade your project to newer template version, template changes will be merged to your project without overwriting your changes. Sometimes merge conflict might occur.

### Templates

Cynosura.Studio creates projects from default template [Cynosura.Template](https://github.com/CynosuraPlatform/Cynosura.Template). However you can use custom templates. 

If you want to create your own template then start by forking Cynosura.Template. Your template must be published in some nuget feed to use in Cynosura.Studio. To use custom template configure it in appsettings.json in Templates section.

### NuGet Feeds

Cynosura.Studio supports private NuGet feeds in appsettings with keys Nuget/FeedUrl, and Nuget/Username and Nuget/Password.

For Template development, you can set local folder feed LocalFeed/SourcePath.

## CLI Tool (`cyn`)

Cynosura.Studio includes a command-line tool distributed as a .NET global tool. It allows you to create, generate, list, upgrade, and inspect Cynosura solutions directly from the terminal without running the web UI.

### Installation

```bash
dotnet tool install -g Cynosura.Studio.CliTool
```

### General Syntax

```
cyn <command> [command arguments] [options]
```

### Commands

#### `cyn new` — Create a new solution

Creates a new Cynosura solution in the current (or specified) directory.

```bash
cyn new <name> --templateName <templateName>
```

- `<name>` — name of the solution to create.
- `--templateName` — (optional) template to use. Defaults to `Cynosura.Template`.

#### `cyn list` — List entities or enums

Displays a table of entities or enums defined in the solution metadata.

```bash
cyn list entity       # or: cyn list entities
cyn list enum         # or: cyn list enums
```

#### `cyn generate` — Generate code from metadata

Generates code for an entity, an enum, or everything at once.

```bash
cyn generate entity <entityName>
cyn generate enum <enumName>
cyn generate all
```

- `generate all` iterates over every non-abstract entity and every enum in the solution and generates code for each.

#### `cyn upgrade` — Upgrade solution template

Upgrades the current solution to the latest version of its template. Template changes are merged without overwriting your custom modifications.

```bash
cyn upgrade
```

#### `cyn update` — Update generated code after metadata changes

Regenerates code for an entity or enum based on the current git changes to its metadata JSON file. It reads the previous version from git history (`HEAD`) and the current version from disk, then runs the upgrade generators to merge changes into your code without overwriting custom modifications.

```bash
cyn update entity <entityName>
cyn update enum <enumName>
```

#### `cyn info` — Show solution and tool information

Prints solution name, template name, template version, CLI version, and CLI location.

```bash
cyn info
```

#### `cyn help` — Display help

```bash
cyn help              # list all commands
cyn help <command>    # show help for a specific command
```

### Global Options

These options can be appended to any command:

| Option | Description |
|---|---|
| `--solutionDirectory <path>` | Set the solution directory (alias: `--solution`). Defaults to the current directory. |
| `--feed <url>` | NuGet feed URL. Defaults to `https://api.nuget.org/v3/index.json`. |
| `--src <path>` | Local feed source path for template development. |
| `--templateName <name>` | Template package name. Defaults to `Cynosura.Template`. |
| `--log <level>` | Set log level (`Trace`, `Debug`, `Information`, `Warning`, `Error`). |
| `--debug` | Shorthand for `--log Debug`. |
| `-v` | Verbosity: Warning level. |
| `-vv` | Verbosity: Information level. |
| `-vvv` | Verbosity: Debug level. |
| `-vvvv` | Verbosity: Trace level. |
| `--set <expr>` | Override configuration values. Format: `key=value` or `key1=value1,key2=value2`. Dot notation maps to config sections (e.g., `Nuget.FeedUrl=https://...`). |

### Configuration

The CLI reads additional settings from `~/.cynosura/appsettings.json` (user profile directory). You can place NuGet feed credentials or other configuration there instead of passing them as command-line options every time.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md).

## Roadmap

Features to be implemented:

1. Allow to have a choice for frontend implementation
2. Add Blazor WebAssembly frontend

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.