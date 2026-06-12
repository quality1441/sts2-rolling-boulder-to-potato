# Rolling Boulder to Potato - A Slay the Spire II mod

[![Video Title](https://img.youtube.com/vi/47essAxLv78/0.jpg)](https://www.youtube.com/watch?v=47essAxLv78)

A cosmetic [Slay the Spire 2](https://store.steampowered.com/app/2868840/Slay_the_Spire_2/) mod that replaces the **Rolling Boulder** card portrait, power icon beneath your character, and the rolling combat VFX with one of three potatoes.

## Config

Edit `config.json` in the mod folder and restart the game:

```json
{
  "potato": "random"
}
```

| `"potato"` | Behavior |
|---|---|
| `"random"` | Pick potato 1, 2, or 3 when the game launches (default) |
| `1` | Cartoon potato |
| `2` | Red potato |
| `3` | Russet potato |

## Install (players)

Extract the `Sts2RollingBoulderToPotato` folder into:

```
<STS2 install>\mods\Sts2RollingBoulderToPotato\
```

Restart STS2 with mods enabled.

## Build and deploy (developers)

Requirements:

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Slay the Spire 2 installed (references `sts2.dll` from your game folder)

If STS2 is not in the default Steam location, either:

- Set `$env:STS2_GAME_ROOT = 'C:\path\to\Slay the Spire 2'`, or
- Copy `local.props.template` to `local.props` and edit the path

```powershell
.\scripts\deploy-mod.ps1
```

This builds Release and copies the mod to `<STS2>\mods\Sts2RollingBoulderToPotato\`.

### Release package (for GitHub / Nexus)

Builds a clean `dist\Sts2RollingBoulderToPotato\` folder plus a versioned zip:

```powershell
.\scripts\release-mod.ps1
```

Use `-NoZip` if you only want the dist folder. Upload the zip or zip contents to your release page.

## Repository layout

```
Sts2RollingBoulderToPotatoMod/   # mod source + embedded potato PNGs
scripts/                           # build/deploy helpers
config.example.json                # default config shipped with releases
mod-thumbnail.jpg                  # optional mod list image
```

Build artifacts (`.godot/`, `dist/`, etc.) are gitignored.

## Notes

- Cosmetic only (`affects_gameplay: false`)
- Modded and unmodded saves are separate
- Config changes require a full game restart
- Inspired from chat interaction on [Frost Prime's STS2](https://www.youtube.com/watch?v=ffSVUOa5CIc) live stream.
- **Tested with STS2 v0.103.3** (Early Access). New game patches may require a mod update.

## Disclaimer

*Slay the Spire 2* and related trademarks are property of Mega Crit Games. This project is a fan/community resource and is **not** affiliated with or endorsed by Mega Crit or Frost Prime. Use game assets and data in line with applicable terms and fair-use norms for your use case.

## License

Not specified. Modding STS2 is subject to Mega Crit’s modding terms.

<img width="671" height="885" alt="cartoon-potato-card" src="https://github.com/user-attachments/assets/551edd16-657e-454e-a117-5b595a370593" />
<img width="674" height="884" alt="red-potato-card" src="https://github.com/user-attachments/assets/d35e3ec2-e3c7-4d51-b5b2-db80f7c4eb43" />
<img width="674" height="892" alt="russet-potato-card" src="https://github.com/user-attachments/assets/dc3c66bd-7eae-47c6-a918-696a5cb5ee6c" />
