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
- Save/load characters
- Remote sessions (see below)

## Future Capabilities
- Track other things for characters
  - Attributes
  - Skills
  - Gear
- Save/load encounters
- Import character from Chummer5
- Port UI to .NET MAUI and support more OSs.

## Supported Platforms

Windows 10+

## Remote Connections

The project `ShadowrunTracker.Api` is a bare-bones AspNetCore WebAPI project that has a SignalR hub for real-time updates of initiative between
different instances of the tracker.
You can start the tracker in three modes
- Start Local: This is the basic way to run the tracker. It has a purely local and in-process state.
- Start Remote: This will attempt to connect to the SignalR hub and start an encounter as the GM. Updates the encounter's state will be sent to the
to any connected clients via the hub.
- Join Remote: This will start a purely read-only session. Select from the list of active encounters and it will sync itself to the GM's state. The main window will then minimize and a new, mostly transparent, window will show the current initiative pass and the damage tracks of PCs.

## Building
Requirements:
- Microsoft Visual Studio 2022 (on Windows)
  - With at least _Desktop Development_ componetns selected.

_All third party packages used are available on nuget._

It's aslo possible to build it using the `dotnet` command line, which is what CI will use once/if I get that set up.
