using BoostsPlugin.Components;
using BoostsPlugin.Modifiers;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostsPlugin
{
    public class BoostsPlugin : RocketPlugin<BoostsConfiguration>
    {
        public static BoostsPlugin Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;

            if (Level.isLoaded)
            {
                ItemClothingModifier.ApplyArmors(Configuration.Instance.ArmorClothings);
            } else
            {
                Level.onLevelLoaded += (_) => ItemClothingModifier.ApplyArmors(Configuration.Instance.ArmorClothings);
            }

            U.Events.OnPlayerConnected += OnPlayerConnected;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been unloaded!", ConsoleColor.Yellow);
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            player.Player.gameObject.AddComponent<PlayerBoostsComponent>();
        }
    }
}
