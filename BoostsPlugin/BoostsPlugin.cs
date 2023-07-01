using RestoreMonarchy.Boosts.Components;
using RestoreMonarchy.Boosts.Modifiers;
using RestoreMonarchy.Boosts.Services;
using HarmonyLib;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using RestoreMonarchy.Boosts.Helpers;

namespace RestoreMonarchy.Boosts
{
    public class BoostsPlugin : RocketPlugin<BoostsConfiguration>
    {
        public static BoostsPlugin Instance { get; private set; }

        public ArmorsDurabilityService ArmorsDurabilityService { get; private set; }

        public const string HarmonyInstanceId = "com.restoremonarchy.boosts";
        private Harmony Harmony { get; set; }

        protected override void Load()
        {
            Instance = this;
            Harmony = new Harmony(HarmonyInstanceId);
            Harmony.PatchAll(Assembly);

            if (Configuration.Instance.UseArmorDurabilities)
            {
                ArmorsDurabilityService = gameObject.AddComponent<ArmorsDurabilityService>();
            }
                

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
            
            PlayerLife.onPlayerDied += OnPlayerDied;
            UnturnedPlayerEvents.OnPlayerWear += OnPlayerWear;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            Harmony.UnpatchAll();
            Destroy(ArmorsDurabilityService);

            U.Events.OnPlayerConnected -= OnPlayerConnected;
            VehicleManager.onEnterVehicleRequested -= OnEnterVehicleRequested;
            VehicleManager.onSwapSeatRequested -= OnSwapSeatRequested;

            PlayerLife.onPlayerDied -= OnPlayerDied;
            UnturnedPlayerEvents.OnPlayerWear -= OnPlayerWear;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been unloaded!", ConsoleColor.Yellow);
        }

        private void OnPlayerWear(UnturnedPlayer player, UnturnedPlayerEvents.Wearables wear, ushort id, byte? quality)
        {
            var comp = player.GetComponent<PlayerBoostsComponent>();
            
            if (comp == null)
            {
                return;
            }

            if (comp.Clothings[wear] != 0)
            {
                comp.RemoveBoost(comp.Clothings[wear], true);
            }

            if (id != 0)
            {
                comp.ApplyBoost(id, true);
                comp.Clothings[wear] = id;
            }
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
            ThreadHelper.QueueOnMainThread(() =>
            {
                if (player.Player == null)
                {
                    return;
                }

                player.Player.gameObject.AddComponent<PlayerBoostsComponent>();
                player.Player.gameObject.AddComponent<DefaultUIComponent>();
            }, 1);
        }

        private void OnPlayerDied(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            sender.GetComponent<PlayerBoostsComponent>().ResetBoosters();
        }
    }
}
