using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;

namespace FullscreenDeSmuME
{
    public class FullscreenDeSmuMESettings : ISettings
    {
        [JsonIgnore]
        private FullscreenDeSmuMEPlugin plugin;
        [JsonIgnore]
        private IPlayniteAPI api;

        public FullscreenDeSmuMESettings()
        {
        }

        public FullscreenDeSmuMESettings(FullscreenDeSmuMEPlugin plugin, IPlayniteAPI api)
        {
            this.plugin = plugin;
            this.api = api;

            var savedSettings = plugin.LoadPluginSettings<FullscreenDeSmuMESettings>();
            if (savedSettings != null)
            {
                SelectedEmulatorGuid = savedSettings.SelectedEmulatorGuid;
                Timeout = savedSettings.Timeout;
            }
        }

        [JsonIgnore]
        public IItemCollection<Emulator> Emulators { get; set; }
        [JsonIgnore]
        public Emulator SelectedEmulator { get; set; }
        [JsonIgnore]
        public string TimeoutText { get; set; }

        public int Timeout { get; set; } = 1000;
        public Guid SelectedEmulatorGuid { get; set; }

        public void BeginEdit()
        {
            Emulators = api.Database.Emulators;
            CancelEdit();
        }

        public void CancelEdit()
        {
            SelectedEmulator = Emulators.FirstOrDefault(emulator => emulator.Id == SelectedEmulatorGuid);
            TimeoutText = Timeout.ToString();
        }

        public void EndEdit()
        {
            SelectedEmulatorGuid = SelectedEmulator == null ? Guid.Empty : SelectedEmulator.Id;
            plugin.SavePluginSettings(this);
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = new List<string>();

            try
            {
                TimeoutText = Regex.Replace(TimeoutText, "[^0-9]", "");
                Timeout = int.Parse(TimeoutText);
                return true;
            }
            catch
            {
                errors.Add(string.Format("\"{0}\" couldn't be converted into an integer!", TimeoutText));
                return false;
            }
        }
    }
}
