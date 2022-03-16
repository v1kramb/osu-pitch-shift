# osu-pitch-shift
Simple mod that adds the ability to globally shift the pitch of songs in osu!

## Instructions
Place ModPitchShift.cs in osu.Game/Rulesets/Mods
<br><br>
For whatever game mode you are trying to add this to:
- Go to osu.Game.Rulesets.{GameMode}/{GameMode}Ruleset.cs
- Scroll to "case ModType.Conversion:"
- Add "new ModPitchShift()"
