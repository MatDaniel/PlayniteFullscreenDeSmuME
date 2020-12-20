# Playnite Fullscreen DeSmuME
This is the repository for the Playnite Generic plugin "Fullscreen DeSmuME". Are you annoyed that there is seemingly no way to get DeSmuME into full screen mode? This plugin tries to solve the issue!

## How does it work?
1. Each time a game is started, it checks whether the emulator matches the one specified in the settings.
2. Then the number of milliseconds specified in the settings is waited for (default: 1000ms).
3. Finally, the DeSmuME window is brought to the foreground and the key combination [ALT]+[RETURN] is executed.

## Building
Execute the file "build.ps1" in the "build" directory.
