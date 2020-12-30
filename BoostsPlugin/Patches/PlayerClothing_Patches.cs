using BoostsPlugin.Components;
using HarmonyLib;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostsPlugin.Patches
{
    [HarmonyPatch(typeof(PlayerClothing))]
    class PlayerClothing_Patches
    {
        [HarmonyPatch("sendUpdateShirtQuality")]
        [HarmonyPrefix]
        static void sendUpdateShirtQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<PlayerUIComponent>().RefreshShirtArmor();
        }

        [HarmonyPatch("sendUpdatePantsQuality")]
        [HarmonyPrefix]
        static void sendUpdatePantsQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<PlayerUIComponent>().RefreshPantsArmor();
        }

        [HarmonyPatch("sendUpdateVestQuality")]
        [HarmonyPrefix]
        static void sendUpdateVestQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<PlayerUIComponent>().RefreshVestArmor();
        }

        [HarmonyPatch("sendUpdateHatQuality")]
        [HarmonyPrefix]
        static void sendUpdateHatQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<PlayerUIComponent>().RefreshHatArmor();
        }
    }
}
