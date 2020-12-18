using BoostsPlugin.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoostsPlugin.Components
{
    public class PlayerBoostsComponent : MonoBehaviour
    {
        private BoostsPlugin pluginInstance => BoostsPlugin.Instance;

        public Player Player { get; private set; }

        public List<BoostItem> Boosters { get; private set; }

        public BoostItem CurrentSpeedBooster { get; set; }
        public BoostItem CurrentJumpBooster { get; set; }

        public void DefaultSpeedBooster()
        {
            Player.movement.sendPluginSpeedMultiplier(1);
            CurrentSpeedBooster = null;
        }

        public void DefaultJumpBooster()
        {
            Player.movement.sendPluginJumpMultiplier(1);
            CurrentSpeedBooster = null;
        }

        public void ChangeSpeedBooster(BoostItem booster)
        {
            Player.movement.sendPluginSpeedMultiplier(booster.SpeedBoost);
            CurrentSpeedBooster = booster;
        }

        public void ChangeJumpBooster(BoostItem booster)
        {
            Player.movement.sendPluginJumpMultiplier(booster.JumpBoost);
            CurrentJumpBooster = booster;
        }

        void Awake()
        {
            Player = GetComponent<Player>();
            Boosters = new List<BoostItem>();
        }
        
        void Start()
        {
            Player.equipment.onEquipRequested += OnEquipRequested;
            Player.inventory.onInventoryAdded += OnInventoryAdded;
            Player.inventory.onInventoryRemoved += OnInventoryRemoved;
        }

        void OnDestroy()
        {
            Player.equipment.onEquipRequested -= OnEquipRequested;
            Player.inventory.onInventoryAdded -= OnInventoryAdded;
            Player.inventory.onInventoryRemoved -= OnInventoryRemoved;
        }

        private void OnEquipRequested(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow)
        {
            ApplyBoost(asset.id, true);
        }

        private void OnInventoryAdded(byte page, byte index, ItemJar jar)
        {
            ApplyBoost(jar.item.id, false);
        }

        private void OnInventoryRemoved(byte page, byte index, ItemJar jar)
        {
            RemoveBoost(jar.item.id);
        }

        public void ApplyBoost(ushort itemId, bool isEquip)
        {
            var booster = pluginInstance.Configuration.Instance.BoosterItems.FirstOrDefault(x => x.ItemId == itemId);
            if (booster == null)
                return;

            if (!isEquip || (booster.RequireEquip && isEquip))
            {
                if (booster.SpeedBoost > Player.movement.pluginSpeedMultiplier)
                {
                    ChangeSpeedBooster(booster);
                }
                    
                if (booster.JumpBoost > Player.movement.pluginJumpMultiplier)
                {
                    ChangeJumpBooster(booster);
                }                    
            }

            Boosters.Add(booster);
        }

        public void RemoveBoost(ushort itemId)
        {
            var booster = pluginInstance.Configuration.Instance.BoosterItems.FirstOrDefault(x => x.ItemId == itemId);
            if (booster == null)
                return;

            Boosters.Remove(booster);

            if (CurrentJumpBooster == booster || CurrentSpeedBooster == booster)
            {
                UseBestBoosters();
            }
        }

        public void UseBestBoosters()
        {
            BoostItem bestSpeedBooster = Boosters.FirstOrDefault();
            BoostItem bestJumpBooster = Boosters.FirstOrDefault();

            foreach (var booster in Boosters)
            {
                if (booster.SpeedBoost > bestSpeedBooster.SpeedBoost)
                    bestSpeedBooster = booster;
                if (booster.JumpBoost > bestJumpBooster.JumpBoost)
                    bestJumpBooster = booster;
            }
                
            if (bestSpeedBooster == null && bestJumpBooster == null)
            {
                DefaultSpeedBooster();
                DefaultJumpBooster();                
            }

            ChangeSpeedBooster(bestSpeedBooster);
            ChangeJumpBooster(bestJumpBooster);
        }

        public void Refresh()
        {
            Boosters.Clear();

            for (byte page = 0; page < 7; page++)
            {
                for (byte index = 0; index < Player.inventory.getItemCount(page); index++)
                {
                    ApplyBoost(Player.inventory.getItem(page, index).item.id, false);
                }
            }

            ApplyBoost(Player.equipment.asset.id, true);
        }
    }
}
