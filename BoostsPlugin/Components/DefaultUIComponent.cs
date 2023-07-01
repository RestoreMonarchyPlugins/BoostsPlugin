using RestoreMonarchy.Boosts.Helpers;
using RestoreMonarchy.Boosts.Models;
using SDG.NetTransport;
using SDG.Unturned;
using UnityEngine;

namespace RestoreMonarchy.Boosts.Components
{
    public class DefaultUIComponent : MonoBehaviour, IArmorsUI
    {
        private BoostsPlugin pluginInstance => BoostsPlugin.Instance;

        public const short Key = 8911;

        public Player Player { get; private set; }

        private ITransportConnection TransportConnection => Player.channel.GetOwnerTransportConnection();

        public PlayerBoostsComponent BoostsComponent { get; private set; }

        void Awake()
        {
            Player = GetComponent<Player>();
            BoostsComponent = GetComponent<PlayerBoostsComponent>();
        }

        void Start()
        {
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.DefaultEffectId, Key, TransportConnection, true);
            RefreshArmorUI();
            RefreshBoostsUI();

            Player.clothing.onHatUpdated += RefreshHatArmor;
            Player.clothing.onShirtUpdated += RefreshShirtArmor;
            Player.clothing.onVestUpdated += RefreshVestArmor;
            Player.clothing.onPantsUpdated += RefreshPantsArmor;

            BoostsComponent.OnBoostUpdated += RefreshBoostsUI;
        }

        void OnDestroy()
        {
            Player.clothing.onHatUpdated -= RefreshHatArmor;
            Player.clothing.onShirtUpdated -= RefreshShirtArmor;
            Player.clothing.onVestUpdated -= RefreshVestArmor;
            Player.clothing.onPantsUpdated -= RefreshPantsArmor;

            BoostsComponent.OnBoostUpdated -= RefreshBoostsUI;

            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.DefaultEffectId, TransportConnection);
        }

        private void RefreshArmorUI()
        {
            RefreshHatArmor();
            RefreshShirtArmor();
            RefreshVestArmor();
            RefreshPantsArmor();
        }

        private void RefreshHatArmor(ushort a, byte b, byte[] c) => RefreshHatArmor();
        public void RefreshHatArmor()
        {
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "Text0", FormatHelper.GetArmorString(Player.clothing.hatAsset?.armor ?? 1, Player.clothing.hatQuality));
            if (Player.clothing.hat == 0)
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item0", false);
            } else
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item0", true);
            }
        }

        private void RefreshShirtArmor(ushort a, byte b, byte[] c) => RefreshShirtArmor();
        public void RefreshShirtArmor()
        {
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "Text1", FormatHelper.GetArmorString(Player.clothing.shirtAsset?.armor ?? 1, Player.clothing.shirtQuality));
            if (Player.clothing.shirt == 0)
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item1", false);
            } else
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item1", true);
            }
        }

        private void RefreshVestArmor(ushort a, byte b, byte[] c) => RefreshVestArmor();
        public void RefreshVestArmor()
        {
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "Text2", FormatHelper.GetArmorString(Player.clothing.vestAsset?.armor ?? 1, Player.clothing.vestQuality));
            if (Player.clothing.vest == 0)
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item2", false);
            } else
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item2", true);
            }
        }

        private void RefreshPantsArmor(ushort a, byte b, byte[] c) => RefreshPantsArmor();
        public void RefreshPantsArmor()
        {
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "Text3", FormatHelper.GetArmorString(Player.clothing.pantsAsset?.armor ?? 1, Player.clothing.pantsQuality));
            if (Player.clothing.pants == 0)
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item3", false);
            }                
            else
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item3", true);
            }   
        }

        private void RefreshBoostsUI()
        {
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "Text4", FormatHelper.GetBoostString(Player.movement.pluginSpeedMultiplier));

            if (Player.movement.pluginSpeedMultiplier == 1)
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item4", false);
            } else
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item4", true);
            }                

            EffectManager.sendUIEffectText(Key, TransportConnection, true, "Text5", FormatHelper.GetBoostString(Player.movement.pluginJumpMultiplier));

            if (Player.movement.pluginJumpMultiplier == 1)
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item5", false);
            } else
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "Item5", true);
            }
        }        
    }
}
