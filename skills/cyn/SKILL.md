---
name: cyn
description: Command-line tool for creating, generating, listing, upgrading, and inspecting Cynosura solutions
---

# Cynosura CLI (`cyn`)

Cynosura.Studio includes a command-line tool (`cyn`) distributed as a .NET global tool. It allows you to create, generate, list, upgrade, and inspect Cynosura solutions directly from the terminal without running the web UI.

## Installation

```bash
dotnet tool install -g Cynosura.Studio.CliTool
```

## General Syntax

```
cyn <command> [command arguments] [options]
```

## Commands

### `cyn new` — Create a new solution

Creates a new Cynosura solution in the current (or specified) directory.

```bash
cyn new <name> --templateName <templateName>
```

- `<name>` — name of the solution to create.
- `--templateName` — (optional) template to use. Defaults to `Cynosura.Template`.

### `cyn list` — List entities or enums

Displays a table of entities or enums defined in the solution metadata.

```bash
cyn list entity       # or: cyn list entities
cyn list enum         # or: cyn list enums
```

### `cyn generate` — Generate code from metadata

Generates code for an entity, an enum, or everything at once.

```bash
cyn generate entity <entityName>
cyn generate enum <enumName>
cyn generate all
```

- `generate all` iterates over every non-abstract entity and every enum in the solution and generates code for each.

### `cyn upgrade` — Upgrade solution template

Upgrades the current solution to the latest version of its template. Template changes are merged without overwriting your custom modifications.

```bash
cyn upgrade
```

### `cyn update` — Update generated code after metadata changes

Regenerates code for an entity or enum based on the current git changes to its metadata JSON file. It reads the previous version from git history (`HEAD`) and the current version from disk, then runs the upgrade generators to merge changes into your code without overwriting custom modifications.

```bash
cyn update entity <entityName>
cyn update enum <enumName>
```

### `cyn info` — Show solution and tool information

Prints solution name, template name, template version, CLI version, and CLI location.

```bash
cyn info
```

### `cyn help` — Display help

```bash
cyn help              # list all commands
cyn help <command>    # show help for a specific command
```

## Global Options

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

## Configuration

The CLI reads additional settings from `~/.cynosura/appsettings.json` (user profile directory). You can place NuGet feed credentials or other configuration there instead of passing them as command-line options every time.
