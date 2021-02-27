using RestoreMonarchy.BoostsPlugin.Helpers;
using RestoreMonarchy.BoostsPlugin.Models;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace RestoreMonarchy.BoostsPlugin.Components
{
    public class RiseUIComponent : MonoBehaviour, IArmorsUI
    {
        private BoostsPlugin pluginInstance => BoostsPlugin.Instance;

        public const short Key = 24523;

        public Player Player { get; private set; }

        private CSteamID CSteamID => Player.channel.owner.playerID.steamID;

        public PlayerBoostsComponent BoostsComponent { get; private set; }

        void Awake()
        {
            Player = GetComponent<Player>();
            BoostsComponent = GetComponent<PlayerBoostsComponent>();
        }

        void Start()
        {
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.RiseEffectId, Key, CSteamID, true);
            RefreshArmorUI();
            RefreshBoostsUI();

            Player.clothing.onHatUpdated += RefreshArmorUI;
            Player.clothing.onShirtUpdated += RefreshArmorUI;
            Player.clothing.onVestUpdated += RefreshArmorUI;
            Player.clothing.onPantsUpdated += RefreshArmorUI;

            BoostsComponent.OnBoostUpdated += RefreshBoostsUI;
        }

        void OnDestroy()
        {
            Player.clothing.onHatUpdated -= RefreshArmorUI;
            Player.clothing.onShirtUpdated -= RefreshArmorUI;
            Player.clothing.onVestUpdated -= RefreshArmorUI;
            Player.clothing.onPantsUpdated -= RefreshArmorUI;

            BoostsComponent.OnBoostUpdated -= RefreshBoostsUI;

            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.RiseEffectId, CSteamID);
        }

        private void RefreshArmorUI(ushort a, byte b, byte[] c)
        {
            RefreshArmorUI();
        }

        private void RefreshArmorUI()
        {
            if ((Player.clothing.hatAsset == null || Player.clothing.hatAsset.armor == 1)
                && (Player.clothing.shirtAsset == null || Player.clothing.shirtAsset.armor == 1)
                && (Player.clothing.vestAsset == null || Player.clothing.vestAsset.armor == 1) 
                && (Player.clothing.pantsAsset == null || Player.clothing.pantsAsset.armor == 1))
            {
                EffectManager.sendUIEffectVisibility(Key, CSteamID, true, "Armor", false);
                return;
            }

            EffectManager.sendUIEffectVisibility(Key, CSteamID, true, "Armor", true);

            RefreshHatArmor();
            RefreshShirtArmor();
            RefreshVestArmor();
            RefreshPantsArmor();
        }

        public void RefreshHatArmor()
        {
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor0", FormatHelper.GetArmorString(Player.clothing.hatAsset?.armor ?? 1, Player.clothing.hatQuality));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion0", FormatHelper.GetArmorString(Player.clothing.hatAsset?.explosionArmor ?? 1, Player.clothing.hatQuality));
        }

        public void RefreshShirtArmor()
        {
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor1", FormatHelper.GetArmorString(Player.clothing.shirtAsset?.armor ?? 1, Player.clothing.shirtQuality));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion1", FormatHelper.GetArmorString(Player.clothing.shirtAsset?.explosionArmor ?? 1, Player.clothing.shirtQuality));
        }

        public void RefreshVestArmor()
        {
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor2", FormatHelper.GetArmorString(Player.clothing.vestAsset?.armor ?? 1, Player.clothing.vestQuality));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion2", FormatHelper.GetArmorString(Player.clothing.vestAsset?.explosionArmor ?? 1, Player.clothing.vestQuality));
        }

        public void RefreshPantsArmor()
        {
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor3", FormatHelper.GetArmorString(Player.clothing.pantsAsset?.armor ?? 1, Player.clothing.pantsQuality));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion3", FormatHelper.GetArmorString(Player.clothing.pantsAsset?.explosionArmor ?? 1, Player.clothing.pantsQuality));
        }

        private void RefreshBoostsUI()
        {
            if (Player.movement.pluginSpeedMultiplier == 1 && Player.movement.pluginJumpMultiplier == 1)
            {
                EffectManager.sendUIEffectVisibility(Key, CSteamID, true, "Boosts", false);
                return;
            }

            EffectManager.sendUIEffectVisibility(Key, CSteamID, true, "Boosts", true);

            EffectManager.sendUIEffectText(Key, CSteamID, true, "Speed1", FormatHelper.GetBoostString(Player.movement.pluginSpeedMultiplier));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Jump1", FormatHelper.GetBoostString(Player.movement.pluginJumpMultiplier));
        }
    }
}
