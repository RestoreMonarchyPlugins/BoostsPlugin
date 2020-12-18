using BoostsPlugin.Models;
using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BoostsPlugin.Modifiers
{
    public class ItemClothingModifier
    {
        public static void ApplyArmors(ArmorClothing[] armorClothings)
        {
            var armorField = typeof(ItemClothingAsset).GetField("_amor", BindingFlags.NonPublic | BindingFlags.Instance);
            var explosionArmorField = typeof(ItemClothingAsset).GetField("_explosionArmor", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var armorClothing in armorClothings)
            {
                var asset = Assets.find(EAssetType.ITEM, armorClothing.ItemId) as ItemClothingAsset;

                if (asset != null)
                {
                    armorField.SetValue(asset, armorClothing.Armor);
                    explosionArmorField.SetValue(asset, armorClothing.ExplosionArmor);
                    Logger.Log($"[Modification] Changed {asset.itemName} Armor to {armorClothing.Armor} and ExplosionArmor to {armorClothing.ExplosionArmor}");
                }
            }
        }
    }
}
