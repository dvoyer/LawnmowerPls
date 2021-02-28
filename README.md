# LawnmowerPls
A Hollow Knight mod to track grass% completion.

## Installation
Requires the modding api.
Either compile the source or use the precompiled .dll and place it in [hollow knight install]\hollow_knight_Data\Managed\Mods.
Or use the mod installer, found [here](https://radiance.host/mods/ModInstaller.exe)! (may be out of date)

## Version History
- v 2.0 - Cross-checks grass with all cuttable grass in the game, manually checked. UI implemented that tracks grass cut per room, and tracks grass cut overall in the game. Not backwards compatible with v.1.5 or earlier.
- v.1.5 - Introduced UI.
- v 1.0 - Stores all grass cut in the save file, no UI, but full actual functionality.

## Known Issues
- The UI only updates when the nail is swung.
- The UI does not properly hide itself during scene transitions.
- The UI does not disappear when the game is quit to main menu.
-- UI issues are scheduled to be resolved in v.2.5.
- Sometimes grass cut within the same frame fails to register properly until the room is reloaded and the grass is cut again.
- Sometimes grass cut with crystal dash fails to register.
-- The game sometimes fails to call any function corresponding to cutting grass while crystal dashing through grass.
-- More investigation is necessary to determine a workaround, resolution scheduled for v.3.0 or later.

## Phantom Grass
There are several instances of grass in the game that are uncuttable, yet according to the game's code should be cuttable. These should have all been culled from the mod as of v.2.0. If you find any grass which does not register as cut, or if you have cut all the grass in the room you are in, but the UI does not indicate you have completed the room after you have swung your nail, submit a bug report here, detailing which room, where in the room if known, and a copy of your save file.

# License
Copyright 2021 Diana Voyer

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.