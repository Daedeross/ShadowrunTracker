# Initiative Tracker for Shadowrun 5th Edition

## Current capabilities
- Add character to encounters.
- Assign stats to these characters pertinant to initiative.
- Display the current Initiative Pass.
  - Highlight the current character.
  - Display PCs and NPCs differently.
  - Visualy indicate characters that have already acted or are not acting in the current pass.
- Handle wounds and healing.
- Handle on-the-fly changes to a character that alter initiative and wounds.
- Handle actions that have an initiative cost (e.g. Full defense).
- Handle new combat rounds
  - Allow manual entry of initatives each round.
  - Allow automatic rolling of initiatives.
  - Handle a character using the _Blitz_ Edge action
  - Handle a character using the _Sieze the Initiative_ Edge action

## Future Capabilities
- Track other things for characters
  - Attributes
  - Skills
  - Gear
- Save/load characters
- Save/load encounters
- Import character from Chummer5

## Supported Platforms

Windows 10+

## Building
Requirements:
- Microsoft Visual Studio 2022 (on Windows)
  - With at least _Desktop Development_ componetns selected.

_All third party packages used are available on nuget._

It's aslo possible to build it using the `dotnet` command line, which is what CI will use once/if I get that set up.
