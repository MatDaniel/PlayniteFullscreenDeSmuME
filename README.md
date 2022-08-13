# Playnite Fullscreen DeSmuME
This is the repository for the Playnite Generic plugin "Fullscreen DeSmuME". Are you annoyed that there is seemingly no way to get DeSmuME into full screen mode? This plugin tries to solve the issue!

## NO LONGER MAINTAINED
This project is no longer maintained. If you are still looking, here is a possible solution:
```powershell
# Wait until the window is probably ready
# If your computer is too slow, try increasing the number
Start-Sleep -Milliseconds 1000;

# Send key combination {Alt} + {Return}
$wshell = New-Object -ComObject wscript.shell;
$wshell.SendKeys('%{ENTER}');
```
Just add this to the post startup script in the emulator configuration to automatically put the emulator into full screen mode.<br>
(Works on Playnite 9.19)

## How does it work?
1. Each time a game is started, it checks whether the emulator matches the one specified in the settings.
2. Then the number of milliseconds specified in the settings is waited for (default: 1000ms).
3. Finally, the DeSmuME window is brought to the foreground and the key combination [ALT]+[RETURN] is executed.

## Building
Execute the file "build.ps1" in the "build" directory.
