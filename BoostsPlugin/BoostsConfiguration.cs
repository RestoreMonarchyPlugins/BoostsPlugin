using BoostsPlugin.Models;
using Rocket.API;

namespace BoostsPlugin
{
    public class BoostsConfiguration : IRocketPluginConfiguration
    {
        public ArmorClothing[] ArmorClothings { get; set; }
        public BoostItem[] BoosterItems { get; set; }

        public void LoadDefaults()
        {
            ArmorClothings = new ArmorClothing[]
            {
                new ArmorClothing()
                {
                    ItemId = 310,
                    Armor = 1.25f,
                    ExplosionArmor = 1.25f
                }
            };
            BoosterItems = new BoostItem[]
            {
                new BoostItem()
                {
                    ItemId = 363,
                    SpeedBoost = 2,
                    JumpBoost = 1.5f,
                    RequireEquip = true
                }
            };
        }
    }
}