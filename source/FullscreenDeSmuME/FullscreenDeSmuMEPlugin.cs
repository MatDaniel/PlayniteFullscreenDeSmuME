using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Controls;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System.Linq;
using System.IO;
using System.Management;

namespace FullscreenDeSmuME
{
    public class FullscreenDeSmuMEPlugin : Plugin
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private FullscreenDeSmuMESettings settings { get; }
        private ILogger logger { get; } = LogManager.GetLogger();

        public override Guid Id { get; } = Guid.Parse("90557cf8-d588-43e3-a57f-c8df5fdbd12f");

        public FullscreenDeSmuMEPlugin(IPlayniteAPI api) : base(api)
        {
            settings = new FullscreenDeSmuMESettings(this, api);
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunView)
        {
            return new FullscreenDeSmuMESettingsView();
        }

        public override void OnGameStarted(Game game)
        {
            if (game.PlayAction.Type == GameActionType.Emulator && game.PlayAction.EmulatorId == settings.SelectedEmulatorGuid)
                new Thread(() =>
                {
                    Process process = GetProcess(PlayniteApi.Database.Emulators.First(e => e.Id == game.PlayAction.EmulatorId).
                                            Profiles.First(profile => profile.Id == game.PlayAction.EmulatorProfileId).Executable);

                    Thread.Sleep(settings.Timeout);

                    if (game.IsRunning)
                    {
                        if (process != null)
                            SetForegroundWindow(process.MainWindowHandle);
                        System.Windows.Forms.SendKeys.SendWait("%{ENTER}");
                    }
                }).Start();

            base.OnGameStarted(game);
        }

        private Process GetProcess(string path)
        {
            path = Path.GetFullPath(path);
            var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            foreach (var item in from p in Process.GetProcesses()
                                 join mo in results.Cast<ManagementObject>()
                                 on p.Id equals (int)(uint)mo["ProcessId"]
                                 select new
                                 {
                                    Process = p,
                                    Path = (string)mo["ExecutablePath"]
                                 })
                if (item.Path != null && Path.GetFullPath(item.Path) == path)
                    return item.Process;
            return null;
        }
    }
}
