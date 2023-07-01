using HarmonyLib;
using RestoreMonarchy.Boosts.Models;
using SDG.Unturned;

namespace RestoreMonarchy.Boosts.Patches
{
    [HarmonyPatch(typeof(PlayerClothing))]
    class PlayerClothing_Patches
    {
        [HarmonyPatch("sendUpdateShirtQuality")]
        [HarmonyPrefix]
        static void sendUpdateShirtQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<IArmorsUI>().RefreshShirtArmor();
        }

        [HarmonyPatch("sendUpdatePantsQuality")]
        [HarmonyPrefix]
        static void sendUpdatePantsQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<IArmorsUI>().RefreshPantsArmor();
        }

        [HarmonyPatch("sendUpdateVestQuality")]
        [HarmonyPrefix]
        static void sendUpdateVestQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<IArmorsUI>().RefreshVestArmor();
        }

        [HarmonyPatch("sendUpdateHatQuality")]
        [HarmonyPrefix]
        static void sendUpdateHatQuality_Prefix(PlayerClothing __instance)
        {
            __instance.GetComponent<IArmorsUI>().RefreshHatArmor();
        }
    }
}
