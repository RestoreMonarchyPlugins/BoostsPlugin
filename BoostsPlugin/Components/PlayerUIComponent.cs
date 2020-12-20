using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoostsPlugin.Components
{
    public class PlayerUIComponent : MonoBehaviour
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
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.EffectId, Key, CSteamID, true);
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

            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.EffectId, CSteamID);
        }

        private void RefreshArmorUI(ushort a, byte b, byte[] c)
        {
            RefreshArmorUI();
        }

        private void RefreshArmorUI()
        {
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor0", GetArmorString(Player.clothing.hatAsset?.armor ?? 1));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion0", GetArmorString(Player.clothing.hatAsset?.explosionArmor ?? 1));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor1", GetArmorString(Player.clothing.shirtAsset?.armor ?? 1));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion1", GetArmorString(Player.clothing.shirtAsset?.explosionArmor ?? 1));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor2", GetArmorString(Player.clothing.vestAsset?.armor ?? 1));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion2", GetArmorString(Player.clothing.vestAsset?.explosionArmor ?? 1));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Armor3", GetArmorString(Player.clothing.pantsAsset?.armor ?? 1));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Explosion3", GetArmorString(Player.clothing.pantsAsset?.explosionArmor ?? 1));
            
        }

        private string GetArmorString(float armor)
        {
            decimal armorDec = Math.Abs((decimal)armor);
            if (armorDec > 1)
            {
                return $"-{(int)((armorDec - 1) * 100)}%";
            }
            else if (armorDec < 1)
            {
                return $"+{(int)((1 - armorDec) * 100)}%";
            } else
            {
                return "0%";
            }
        }

        private void RefreshBoostsUI()
        {
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Speed1", GetBoostString(Player.movement.pluginSpeedMultiplier));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "Jump1", GetBoostString(Player.movement.pluginJumpMultiplier));
        }

        public string GetBoostString(float boost)
        {
            decimal boostDec = (decimal)boost;
            if (boostDec > 1)
            {
                return $"+{(int)((boostDec - 1) * 100)}%";
            } else if (boostDec < 1)
            {
                return $"-{(int)(boostDec * 100)}%";
            } else 
            {
                return "0%";
            }
        }        
    }
}
