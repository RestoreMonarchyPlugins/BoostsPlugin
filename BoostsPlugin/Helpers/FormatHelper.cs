using System;

namespace RestoreMonarchy.Boosts.Helpers
{
    public class FormatHelper
    {
        public static string GetBoostString(float boost)
        {
            decimal boostDec = (decimal)boost;
            if (boostDec > 1)
            {
                return $"+{(int)((boostDec - 1) * 100)}%";
            }
            else if (boostDec < 1)
            {
                return $"-{(int)((1 - boostDec) * 100)}%";
            }
            else
            {
                return "0%";
            }
        }

        public static string GetArmorString(float armor, byte quality)
        {
            decimal armorDec = Math.Abs((decimal)armor);

            if (armorDec > 1)
            {
                return $"-{(int)((armorDec - 1) * 100)}%";
            }
            if (armorDec < 1)
            {
                decimal actualArmor = (1 - armorDec) * 100;
                actualArmor *= (decimal)quality / 100;
                return $"+{(int)actualArmor}%";
            }
            else
            {
                return "0%";
            }
        }
    }
}
