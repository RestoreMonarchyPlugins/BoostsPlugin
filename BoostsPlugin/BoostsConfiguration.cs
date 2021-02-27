using RestoreMonarchy.BoostsPlugin.Models;
using Rocket.API;

namespace RestoreMonarchy.BoostsPlugin
{
    public class BoostsConfiguration : IRocketPluginConfiguration
    {
        public ushort RiseEffectId { get; set; }
        public ushort DefaultEffectId { get; set; }
        public string UITemplate { get; set; }
        public ArmorClothing[] ArmorClothings { get; set; }
        public BoostItem[] BoosterItems { get; set; }

        public void LoadDefaults()
        {
            RiseEffectId = 24523;
            DefaultEffectId = 8911;
            UITemplate = "default";
            ArmorClothings = new ArmorClothing[]
            {
                new ArmorClothing()
                {
                    ItemId = 1010,
                    Armor = 0.3f,
                    ExplosionArmor = 0.1f
                },
                new ArmorClothing()
                {
                    ItemId = 1011,
                    Armor = 0.4f,
                    ExplosionArmor = 0.4f
                },
                new ArmorClothing()
                {
                    ItemId = 1012,
                    Armor = 0.4f,
                    ExplosionArmor = 0.4f
                },
                new ArmorClothing()
                {
                    ItemId = 1013,
                    Armor = 0.2f,
                    ExplosionArmor = 0.1f
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
                },
                new BoostItem()
                {
                    ItemId = 254,
                    SpeedBoost = 5,
                    JumpBoost = 3,
                    RequireEquip = false
                }
            };
        }
    }
}