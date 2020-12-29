using BoostsPlugin.Components;
using BoostsPlugin.Modifiers;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
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
            VehicleManager.onEnterVehicleRequested += OnEnterVehicleRequested;
            VehicleManager.onSwapSeatRequested += OnSwapSeatRequested;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            VehicleManager.onEnterVehicleRequested -= OnEnterVehicleRequested;
            VehicleManager.onSwapSeatRequested -= OnSwapSeatRequested;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been unloaded!", ConsoleColor.Yellow);
        }

        private void OnEnterVehicleRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            if (vehicle.tryAddPlayer(out byte seat, player) && seat == 0)
            {
                player.GetComponent<PlayerBoostsComponent>().RemoveBoost(player.equipment.itemID, true);
            }
        }

        private void OnSwapSeatRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex)
        {
            if (toSeatIndex == 0)
            {
                player.GetComponent<PlayerBoostsComponent>().RemoveBoost(player.equipment.itemID, true);
            }
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            TaskDispatcher.QueueOnMainThread(() =>
            {
                player.Player.gameObject.AddComponent<PlayerBoostsComponent>();
                player.Player.gameObject.AddComponent<PlayerUIComponent>();
            }, 2);
        }
    }
}
