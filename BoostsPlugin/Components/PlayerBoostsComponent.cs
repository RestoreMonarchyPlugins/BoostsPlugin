using BoostsPlugin.Models;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
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

        public List<Booster> Boosters { get; private set; }

        public Booster CurrentSpeedBooster { get; set; }
        public Booster CurrentJumpBooster { get; set; }

        public delegate void BoostUpdated();
        public event BoostUpdated OnBoostUpdated;

        public void ChangeSpeedBooster(Booster booster)
        {
            CurrentSpeedBooster = booster;
            Player.movement.sendPluginSpeedMultiplier(CurrentSpeedBooster?.BoostItem?.SpeedBoost ?? 1);
            OnBoostUpdated?.Invoke();
        }

        public void ChangeJumpBooster(Booster booster)
        {
            CurrentJumpBooster = booster;
            Player.movement.sendPluginJumpMultiplier(CurrentJumpBooster?.BoostItem?.JumpBoost ?? 1);
            OnBoostUpdated?.Invoke();
        }

        void Awake()
        {
            Player = GetComponent<Player>();
            Boosters = new List<Booster>();
        }
        
        void Start()
        {
            Player.equipment.onEquipRequested += OnEquipRequested;
            Player.equipment.onDequipRequested += OnDequipRequested;
            Player.inventory.onInventoryAdded += OnInventoryAdded;
            Player.inventory.onInventoryRemoved += OnInventoryRemoved;

            ReloadBoosters();
        }

        void OnDestroy()
        {
            Player.equipment.onEquipRequested -= OnEquipRequested;
            Player.equipment.onDequipRequested -= OnDequipRequested;
            Player.inventory.onInventoryAdded -= OnInventoryAdded;
            Player.inventory.onInventoryRemoved -= OnInventoryRemoved;
        }

        private void OnEquipRequested(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow)
        {
            ApplyBoost(asset.id, true);
        }

        private void OnDequipRequested(PlayerEquipment equipment, ref bool shouldAllow)
        {
            RemoveBoost(equipment.itemID, true);
        }

        private void OnInventoryAdded(byte page, byte index, ItemJar jar)
        {
            ApplyBoost(jar.item.id, false);
        }

        private void OnInventoryRemoved(byte page, byte index, ItemJar jar)
        {
            RemoveBoost(jar.item.id, false);
        }

        public void ApplyBoost(ushort itemId, bool isEquip)
        {
            var boostItem = pluginInstance.Configuration.Instance.BoosterItems.FirstOrDefault(x => x.ItemId == itemId);
            if (boostItem == null)
                return;


            if (boostItem.RequireEquip && !isEquip)
            {
                return;
            }

            if (isEquip && !boostItem.RequireEquip)
            {
                return;
            }

            var booster = new Booster(boostItem);
            Boosters.Add(booster);

            if (CurrentSpeedBooster == null || boostItem.SpeedBoost > CurrentSpeedBooster.BoostItem.SpeedBoost)
            {
                ChangeSpeedBooster(booster);
            }

            if (CurrentJumpBooster == null || boostItem.JumpBoost > CurrentJumpBooster.BoostItem.JumpBoost)
            {
                ChangeJumpBooster(booster);
            }
        }

        public void RemoveBoost(ushort itemId, bool isEquip)
        {
            var booster = Boosters.FirstOrDefault(x => x.BoostItem.ItemId == itemId);
            if (booster == null)
                return;

            if (isEquip && !booster.BoostItem.RequireEquip)
            {
                return;
            }                

            Boosters.Remove(booster);

            if (CurrentSpeedBooster == booster)
                UseBestSpeedBooster();

            if (CurrentJumpBooster == booster)
                UseBestJumpBooster();
        }

        public void UseBestSpeedBooster()
        {
            Booster bestSpeedBooster = Boosters.ElementAtOrDefault(0);

            for (int i = 1; i < Boosters.Count; i++)
            {
                if (Boosters[i].BoostItem.SpeedBoost > bestSpeedBooster.BoostItem.SpeedBoost)
                    bestSpeedBooster = Boosters[i];
            }
            ChangeSpeedBooster(bestSpeedBooster);
        }

        public void UseBestJumpBooster()
        {
            Booster bestJumpBooster = Boosters.ElementAtOrDefault(0);

            for (int i = 1; i < Boosters.Count; i++)
            {                
                if (Boosters[i].BoostItem.JumpBoost > bestJumpBooster.BoostItem.JumpBoost)
                    bestJumpBooster = Boosters[i];
            }            
            ChangeJumpBooster(bestJumpBooster);
        }

        public void ReloadBoosters()
        {
            Boosters.Clear();

            for (byte page = 0; page < 7; page++)
            {
                for (byte index = 0; index < Player.inventory.getItemCount(page); index++)
                {
                    ApplyBoost(Player.inventory.getItem(page, index).item.id, false);
                }
            }

            if (Player.equipment.isEquipped)
                ApplyBoost(Player.equipment.itemID, true);
        }
    }
}
