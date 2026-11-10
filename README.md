# zMuzy Parachute

[![Build](https://github.com/zMuzy/zMuzy-Parachute/actions/workflows/build.yml/badge.svg)](https://github.com/zMuzy/zMuzy-Parachute/actions/workflows/build.yml) [![Release](https://img.shields.io/github/v/release/zMuzy/zMuzy-Parachute)](https://github.com/zMuzy/zMuzy-Parachute/releases/latest) [![License](https://img.shields.io/github/license/zMuzy/zMuzy-Parachute)](LICENSE)

A small [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) plugin for CS2: hold E (+use) while falling and you glide down slowly. Let go and normal gravity comes back.

## How it works

While a player is in the air, falling faster than 10 units/s and holding E, their gravity drops to 0.1 and their fall speed is capped at 30 units/s. Gravity is reset on landing, release, spawn, death, round start and plugin unload. Bots are ignored.

There is no config file. The numbers are constants at the top of `MuzyParachutePlugin.cs` if you want to change them.

## Requirements

- CS2 dedicated server with Metamod:Source 2.0.
- CounterStrikeSharp 1.0.379 or newer (with-runtime package, .NET 10).

## Install

1. Download `MuzyParachute-vX.Y.Z.zip` from the [latest release](https://github.com/zMuzy/zMuzy-Parachute/releases/latest).
2. Unzip it into `game/csgo/` on the server. You should end up with `game/csgo/addons/counterstrikesharp/plugins/MuzyParachute/MuzyParachute.dll`.
3. Restart the server, or run `css_plugins load MuzyParachute` in the server console.

## Build

You need the .NET 10 SDK.

```bash
dotnet build MuzyParachute.csproj -c Release
```

Pushing a `v*` tag builds the plugin on GitHub Actions and attaches the zip to a release.

## License

GPL-3.0. See [LICENSE](LICENSE).

## Credits

- [zMuzy](https://muzy.io), with AI assistance.
