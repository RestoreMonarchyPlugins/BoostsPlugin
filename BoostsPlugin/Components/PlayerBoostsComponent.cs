using RestoreMonarchy.Boosts.Models;
using Rocket.Unturned.Events;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.Boosts.Components
{
    public class PlayerBoostsComponent : MonoBehaviour
    {
        private BoostsPlugin pluginInstance => BoostsPlugin.Instance;

        public Player Player { get; private set; }

        public List<Booster> Boosters { get; private set; }

        public Dictionary<UnturnedPlayerEvents.Wearables, ushort>  Clothings { get; private set; }

        public Booster CurrentSpeedBooster { get; private set; }
        public Booster CurrentJumpBooster { get; private set; }

        public Booster EquippedBooster { get; private set; }

        public delegate void BoostUpdated();
        public event BoostUpdated OnBoostUpdated;

        public void ChangeSpeedBooster(Booster booster)
        {
            CurrentSpeedBooster = booster;
        }

        public void ChangeJumpBooster(Booster booster)
        {
            CurrentJumpBooster = booster;            
        }

        private void UpdateSpeedBoost()
        {
            float boost = CurrentSpeedBooster?.BoostItem?.SpeedBoost ?? 1;

            if (boost > 1)
            {
                var badBoost = Boosters.OrderBy(x => x.BoostItem.SpeedBoost).FirstOrDefault(x => x.BoostItem.SpeedBoost < 1);

                if (badBoost != null)
                    boost = (badBoost.BoostItem.SpeedBoost + boost) / 2;
            }
            
            Player.movement.sendPluginSpeedMultiplier(boost);
            OnBoostUpdated?.Invoke();
        }

        private void UpdateJumpBoost()
        {
            float boost = CurrentJumpBooster?.BoostItem?.JumpBoost ?? 1;

            if (boost > 1)
            {
                var badBoost = Boosters.OrderBy(x => x.BoostItem.JumpBoost).FirstOrDefault(x => x.BoostItem.JumpBoost < 1);

                if (badBoost != null)
                    boost = (badBoost.BoostItem.JumpBoost + boost) / 2;
            }
            
            Player.movement.sendPluginJumpMultiplier(boost);
            OnBoostUpdated?.Invoke();
        }

        void Awake()
        {
            Player = GetComponent<Player>();
            Boosters = new List<Booster>();
            Clothings = new Dictionary<UnturnedPlayerEvents.Wearables, ushort>();
        }
        
        void Start()
        {
            Player.equipment.onEquipRequested += OnEquipRequested;
            Player.equipment.onDequipRequested += OnDequipRequested;
            Player.inventory.onInventoryAdded += OnInventoryAdded;
            Player.inventory.onInventoryRemoved += OnInventoryRemoved;
            Player.life.onLifeUpdated += OnLifeUpdated;

            try
            {
                ReloadBoosters();
            } catch (Exception e)
            {
                Logger.LogException(e);
            }            
        }

        void OnDestroy()
        {
            Player.equipment.onEquipRequested -= OnEquipRequested;
            Player.equipment.onDequipRequested -= OnDequipRequested;
            Player.inventory.onInventoryAdded -= OnInventoryAdded;
            Player.inventory.onInventoryRemoved -= OnInventoryRemoved;
            Player.life.onLifeUpdated -= OnLifeUpdated;
        }

        private void OnEquipRequested(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow)
        {
            if (EquippedBooster != null)
                RemoveBoost(EquippedBooster.BoostItem.ItemId, true);
            
            EquippedBooster = ApplyBoost(asset.id, true);
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

        private void OnLifeUpdated(bool isDead)
        {
            if (isDead)
            {
                ChangeSpeedBooster(null);
                ChangeJumpBooster(null);
                UpdateSpeedBoost();
                UpdateJumpBoost();
            }
        }

        public Booster ApplyBoost(ushort itemId, bool isEquip)
        {
            var boostItem = pluginInstance.Configuration.Instance.BoosterItems.FirstOrDefault(x => x.ItemId == itemId);
            if (boostItem == null)
                return null;

            if (boostItem.RequireEquip && !isEquip)
            {
                return null;
            }

            if (isEquip && !boostItem.RequireEquip)
            {
                return null;
            }

            var booster = new Booster(boostItem);
            Boosters.Add(booster);

            if (CurrentSpeedBooster == null 
                || (boostItem.SpeedBoost > 1 && boostItem.SpeedBoost > CurrentSpeedBooster.BoostItem.SpeedBoost)
                || (boostItem.SpeedBoost < 1 && boostItem.SpeedBoost < CurrentSpeedBooster.BoostItem.SpeedBoost))
            {
                ChangeSpeedBooster(booster);
            }


            if (CurrentJumpBooster == null 
                || (boostItem.SpeedBoost > 1 && boostItem.JumpBoost > CurrentJumpBooster.BoostItem.JumpBoost)
                || (boostItem.SpeedBoost < 1 && boostItem.JumpBoost < CurrentJumpBooster.BoostItem.JumpBoost))
            {
                ChangeJumpBooster(booster);
            }

            UpdateSpeedBoost();
            UpdateJumpBoost();

            return booster;
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

            UpdateSpeedBoost();
            UpdateJumpBoost();
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

        public void ResetBoosters()
        {
            Boosters.Clear();
            EquippedBooster = null;
        }

        public void ReloadBoosters()
        {
            ResetBoosters();
            Clothings.Clear();

            for (byte page = 0; page < 7; page++)
            {
                for (byte index = 0; index < Player.inventory.getItemCount(page); index++)
                {
                    ApplyBoost(Player.inventory.getItem(page, index).item.id, false);
                }
            }

            if (Player.clothing.shirt != 0)
                ApplyBoost(Player.clothing.shirt, true);
            if (Player.clothing.pants != 0)
                ApplyBoost(Player.clothing.pants, true);
            if (Player.clothing.vest != 0)
                ApplyBoost(Player.clothing.vest, true);
            if (Player.clothing.hat != 0)
                ApplyBoost(Player.clothing.hat, true);
            if (Player.clothing.mask != 0)
                ApplyBoost(Player.clothing.mask, true);
            if (Player.clothing.glasses != 0)
                ApplyBoost(Player.clothing.glasses, true);
            if (Player.clothing.backpack != 0)
                ApplyBoost(Player.clothing.backpack, true);

            Clothings.Add(UnturnedPlayerEvents.Wearables.Shirt, Player.clothing.shirt);
            Clothings.Add(UnturnedPlayerEvents.Wearables.Pants, Player.clothing.pants);
            Clothings.Add(UnturnedPlayerEvents.Wearables.Vest, Player.clothing.vest);
            Clothings.Add(UnturnedPlayerEvents.Wearables.Hat, Player.clothing.hat);
            Clothings.Add(UnturnedPlayerEvents.Wearables.Mask, Player.clothing.mask);
            Clothings.Add(UnturnedPlayerEvents.Wearables.Glasses, Player.clothing.glasses);
            Clothings.Add(UnturnedPlayerEvents.Wearables.Backpack, Player.clothing.backpack);

            if (Player.equipment.isEquipped)
                ApplyBoost(Player.equipment.itemID, true);
        }
    }
}
