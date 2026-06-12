# Rolling Boulder to Potato

A cosmetic mod for [Slay the Spire 2](https://store.steampowered.com/app/2868840/Slay_the_Spire_2/) that replaces **Rolling Boulder** with potatoes:

- Card portrait
- Power icon beneath your character (damage counter unchanged)
- Rolling combat VFX each turn

**Tested with STS2 v0.103.3** (Early Access). New game patches may require a mod update.

## Requirements

- Slay the Spire 2 with **mods enabled**
- A full restart of the game after installing or updating the mod

## Install

1. Download the latest release zip (or copy this folder).
2. Extract into your STS2 mods folder:

   ```
   <Slay the Spire 2 install>\mods\Sts2RollingBoulderToPotato\
   ```

   Example (Steam default):

   ```
   C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2\mods\Sts2RollingBoulderToPotato\
   ```

   This folder must contain:

   - `Sts2RollingBoulderToPotato.dll`
   - `Sts2RollingBoulderToPotato.json`
   - `config.json` (or copy from `config.example.json`)

3. **Fully quit and restart Slay the Spire 2.**

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

## Troubleshooting

**Mod does not appear in the mod list**

- Check that `Sts2RollingBoulderToPotato.dll` and `Sts2RollingBoulderToPotato.json` are inside `mods\Sts2RollingBoulderToPotato\`, not loose in `mods\`.
- Fully restart the game.

**Changes not applied after update**

- Fully exit and restart STS2 after replacing the DLL.

**Logs**

- Windows: `%APPDATA%\SlayTheSpire2\logs\godot.log`
- Search for `Sts2RollingBoulderToPotato`

## Notes

- Cosmetic only — does not affect gameplay or multiplayer compatibility checks
- Modded and unmodded saves are separate

## Disclaimer

*Slay the Spire 2* and related trademarks are property of Mega Crit Games. This is a fan mod and is not affiliated with or endorsed by Mega Crit.

## License

Not specified. Modding STS2 is subject to Mega Crit's modding terms.
